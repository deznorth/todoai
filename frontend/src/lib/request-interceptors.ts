// Request interceptors and middleware for API calls

import type { ApiResponse } from '@/types';

// Request interceptor to add authentication headers
export const withAuth = (request: RequestInit): RequestInit => {
    // Since we're using HTTP-only cookies, no need to add Authorization header
    // The cookies will be sent automatically with credentials: 'include'
    return {
        ...request,
        credentials: 'include',
    };
};

// Request interceptor to add common headers
export const withCommonHeaders = (request: RequestInit): RequestInit => {
    const headers = new Headers(request.headers);
    
    // Add common headers
    headers.set('Content-Type', 'application/json');
    headers.set('Accept', 'application/json');
    
    // Add CSRF protection if needed
    const csrfToken = document.querySelector('meta[name="csrf-token"]')?.getAttribute('content');
    if (csrfToken) {
        headers.set('X-CSRF-Token', csrfToken);
    }

    return {
        ...request,
        headers,
    };
};

// Response interceptor to handle common response patterns
export const handleResponse = async <T>(response: Response): Promise<ApiResponse<T>> => {
    // Handle different content types
    const contentType = response.headers.get('content-type');
    
    if (!response.ok) {
        let errorMessage = `HTTP ${response.status}: ${response.statusText}`;
        
        try {
            if (contentType?.includes('application/json')) {
                const errorData = await response.json();
                errorMessage = errorData.message || errorData.error || errorMessage;
            } else {
                errorMessage = await response.text() || errorMessage;
            }
        } catch {
            // If we can't parse the error response, use the default message
        }

        return {
            success: false,
            error: {
                message: errorMessage,
                code: response.status.toString(),
            },
        };
    }

    // Handle successful responses
    try {
        // Handle empty responses (like DELETE operations)
        if (response.status === 204 || response.headers.get('content-length') === '0') {
            return {
                success: true,
                data: undefined as T,
            };
        }

        // Handle JSON responses
        if (contentType?.includes('application/json')) {
            const data = await response.json();
            return {
                success: true,
                data,
            };
        }

        // Handle text responses
        const text = await response.text();
        return {
            success: true,
            data: text as T,
        };
    } catch (error) {
        return {
            success: false,
            error: {
                message: 'Failed to parse response',
                code: 'PARSE_ERROR',
                details: error,
            },
        };
    }
};

// Retry logic for failed requests
export const withRetry = async <T>(
    requestFn: () => Promise<ApiResponse<T>>,
    maxRetries: number = 3,
    delay: number = 1000
): Promise<ApiResponse<T>> => {
    let lastError: ApiResponse<T> = {
        success: false,
        error: {
            message: 'Max retries exceeded',
            code: 'MAX_RETRIES_EXCEEDED',
        },
    };

    for (let attempt = 0; attempt <= maxRetries; attempt++) {
        try {
            const result = await requestFn();
            
            // If successful, return immediately
            if (result.success) {
                return result;
            }

            // Don't retry for client errors (4xx)
            if (result.error?.code && result.error.code.startsWith('4')) {
                return result;
            }

            lastError = result;

            // Wait before retrying (exponential backoff)
            if (attempt < maxRetries) {
                await new Promise(resolve => setTimeout(resolve, delay * Math.pow(2, attempt)));
            }
        } catch (error) {
            lastError = {
                success: false,
                error: {
                    message: error instanceof Error ? error.message : 'Unknown error',
                    code: 'NETWORK_ERROR',
                    details: error,
                },
            };

            // Wait before retrying
            if (attempt < maxRetries) {
                await new Promise(resolve => setTimeout(resolve, delay * Math.pow(2, attempt)));
            }
        }
    }

    return lastError;
};

// CORS configuration helper
export const getCorsHeaders = (): Record<string, string> => {
    const isProduction = process.env.NODE_ENV === 'production';
    const apiUrl = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';
    
    return {
        'Access-Control-Allow-Origin': isProduction ? apiUrl : '*',
        'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS',
        'Access-Control-Allow-Headers': 'Content-Type, Authorization, X-CSRF-Token',
        'Access-Control-Allow-Credentials': 'true',
    };
};

// Network status detector
export const isOnline = (): boolean => {
    return typeof navigator !== 'undefined' ? navigator.onLine : true;
};

// Connection health checker
export const checkConnection = async (apiUrl: string): Promise<boolean> => {
    try {
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), 5000);
        
        const response = await fetch(`${apiUrl}/health`, {
            method: 'HEAD',
            signal: controller.signal,
        });
        
        clearTimeout(timeoutId);
        return response.ok;
    } catch {
        return false;
    }
};