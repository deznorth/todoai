// API client for .NET backend communication

import type { ApiResponse, AuthRequest, AuthResponse, Task, CreateTaskRequest, UpdateTaskRequest } from '@/types';
import { withAuth, withCommonHeaders, handleResponse, withRetry, isOnline } from './request-interceptors';

class ApiClient {
    private baseUrl: string;
    private timeout: number;

    constructor() {
        this.baseUrl = process.env.NEXT_PUBLIC_API_URL || 'https://localhost:7051';
        this.timeout = parseInt(process.env.NEXT_PUBLIC_API_TIMEOUT || '10000', 10);
    }

    private async request<T>(endpoint: string, options: RequestInit = {}): Promise<ApiResponse<T>> {
        // Check network status
        if (!isOnline()) {
            return {
                success: false,
                error: {
                    message: 'No internet connection',
                    code: 'OFFLINE',
                },
            };
        }

        const url = `${this.baseUrl}${endpoint}`;

        // Apply interceptors
        let config = withCommonHeaders(options);
        config = withAuth(config);

        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), this.timeout);
        config.signal = controller.signal;

        try {
            const response = await fetch(url, config);
            clearTimeout(timeoutId);

            return handleResponse<T>(response);
        } catch (error) {
            clearTimeout(timeoutId);

            if (error instanceof Error) {
                if (error.name === 'AbortError') {
                    return {
                        success: false,
                        error: {
                            message: 'Request timeout',
                            code: 'TIMEOUT',
                        },
                    };
                }

                return {
                    success: false,
                    error: {
                        message: error.message,
                        code: 'NETWORK_ERROR',
                        details: error,
                    },
                };
            }

            return {
                success: false,
                error: {
                    message: 'Unknown error occurred',
                    code: 'UNKNOWN_ERROR',
                    details: error,
                },
            };
        }
    }

    // Request with retry wrapper
    // @ts-expect-error - TODO: Remove once this method is used in components
    private async requestWithRetry<T>(endpoint: string, options: RequestInit = {}, retries: number = 2): Promise<ApiResponse<T>> {
        return withRetry(() => this.request<T>(endpoint, options), retries);
    }

    // Authentication endpoints
    async login(credentials: AuthRequest): Promise<ApiResponse<AuthResponse>> {
        return this.request<AuthResponse>('/auth/login', {
            method: 'POST',
            body: JSON.stringify(credentials),
        });
    }

    async signup(credentials: AuthRequest): Promise<ApiResponse<AuthResponse>> {
        return this.request<AuthResponse>('/auth/signup', {
            method: 'POST',
            body: JSON.stringify(credentials),
        });
    }

    async logout(): Promise<ApiResponse<void>> {
        return this.request<void>('/auth/logout', {
            method: 'POST',
        });
    }

    // Task endpoints
    async getTasks(): Promise<ApiResponse<Task[]>> {
        return this.request<Task[]>('/tasks');
    }

    async getTask(id: string): Promise<ApiResponse<Task>> {
        return this.request<Task>(`/tasks/${id}`);
    }

    async createTask(task: CreateTaskRequest): Promise<ApiResponse<Task>> {
        return this.request<Task>('/tasks', {
            method: 'POST',
            body: JSON.stringify(task),
        });
    }

    async updateTask(id: string, updates: UpdateTaskRequest): Promise<ApiResponse<Task>> {
        return this.request<Task>(`/tasks/${id}`, {
            method: 'PUT',
            body: JSON.stringify(updates),
        });
    }

    async deleteTask(id: string): Promise<ApiResponse<void>> {
        return this.request<void>(`/tasks/${id}`, {
            method: 'DELETE',
        });
    }

    // Health check
    async healthCheck(): Promise<ApiResponse<{ status: string }>> {
        return this.request<{ status: string }>('/health');
    }
}

// Export singleton instance
export const apiClient = new ApiClient();

// Export class for testing
export { ApiClient };
