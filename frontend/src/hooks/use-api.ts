// React hooks for API interactions

import { useState, useCallback } from 'react';
import { apiClient } from '@/lib/api-client';
import type { ApiResponse, AuthRequest, AuthResponse, Task, CreateTaskRequest, UpdateTaskRequest } from '@/types';

interface ApiState<T> {
    data: T | null;
    loading: boolean;
    error: string | null;
}

// Generic hook for API calls
export const useApiCall = <T>() => {
    const [state, setState] = useState<ApiState<T>>({
        data: null,
        loading: false,
        error: null,
    });

    const execute = useCallback(async (apiCall: () => Promise<ApiResponse<T>>) => {
        setState(prev => ({ ...prev, loading: true, error: null }));
        
        try {
            const response = await apiCall();
            
            if (response.success) {
                setState({
                    data: response.data || null,
                    loading: false,
                    error: null,
                });
                return response;
            } else {
                setState({
                    data: null,
                    loading: false,
                    error: response.error?.message || 'An error occurred',
                });
                return response;
            }
        } catch (error) {
            const errorMessage = error instanceof Error ? error.message : 'Unknown error';
            setState({
                data: null,
                loading: false,
                error: errorMessage,
            });
            return {
                success: false,
                error: { message: errorMessage },
            } as ApiResponse<T>;
        }
    }, []);

    const reset = useCallback(() => {
        setState({
            data: null,
            loading: false,
            error: null,
        });
    }, []);

    return {
        ...state,
        execute,
        reset,
    };
};

// Authentication hooks
export const useAuth = () => {
    const loginApi = useApiCall<AuthResponse>();
    const signupApi = useApiCall<AuthResponse>();
    const logoutApi = useApiCall<void>();

    const login = useCallback(async (credentials: AuthRequest) => {
        return loginApi.execute(() => apiClient.login(credentials));
    }, [loginApi]);

    const signup = useCallback(async (credentials: AuthRequest) => {
        return signupApi.execute(() => apiClient.signup(credentials));
    }, [signupApi]);

    const logout = useCallback(async () => {
        return logoutApi.execute(() => apiClient.logout());
    }, [logoutApi]);

    return {
        login: {
            ...loginApi,
            execute: login,
        },
        signup: {
            ...signupApi,
            execute: signup,
        },
        logout: {
            ...logoutApi,
            execute: logout,
        },
    };
};

// Task management hooks
export const useTasks = () => {
    const getTasksApi = useApiCall<Task[]>();
    const getTaskApi = useApiCall<Task>();
    const createTaskApi = useApiCall<Task>();
    const updateTaskApi = useApiCall<Task>();
    const deleteTaskApi = useApiCall<void>();

    const getTasks = useCallback(async () => {
        return getTasksApi.execute(() => apiClient.getTasks());
    }, [getTasksApi]);

    const getTask = useCallback(async (id: string) => {
        return getTaskApi.execute(() => apiClient.getTask(id));
    }, [getTaskApi]);

    const createTask = useCallback(async (task: CreateTaskRequest) => {
        return createTaskApi.execute(() => apiClient.createTask(task));
    }, [createTaskApi]);

    const updateTask = useCallback(async (id: string, updates: UpdateTaskRequest) => {
        return updateTaskApi.execute(() => apiClient.updateTask(id, updates));
    }, [updateTaskApi]);

    const deleteTask = useCallback(async (id: string) => {
        return deleteTaskApi.execute(() => apiClient.deleteTask(id));
    }, [deleteTaskApi]);

    return {
        getTasks: {
            ...getTasksApi,
            execute: getTasks,
        },
        getTask: {
            ...getTaskApi,
            execute: getTask,
        },
        createTask: {
            ...createTaskApi,
            execute: createTask,
        },
        updateTask: {
            ...updateTaskApi,
            execute: updateTask,
        },
        deleteTask: {
            ...deleteTaskApi,
            execute: deleteTask,
        },
    };
};

// Health check hook
export const useHealthCheck = () => {
    const healthApi = useApiCall<{ status: string }>();

    const checkHealth = useCallback(async () => {
        return healthApi.execute(() => apiClient.healthCheck());
    }, [healthApi]);

    return {
        ...healthApi,
        execute: checkHealth,
    };
};