// Type definitions for TodoAI application

export interface User {
    id: string;
    email: string;
    created_at: string;
    updated_at: string;
}

export interface Task {
    id: string;
    user_id: string;
    title: string;
    description?: string;
    due_date?: string;
    priority: TaskPriority;
    tags?: string[];
    recurrence?: TaskRecurrence;
    is_deleted: boolean;
    created_at: string;
    updated_at: string;
}

export type TaskPriority = 'low' | 'medium' | 'high';
export type TaskRecurrence = 'none' | 'daily' | 'weekly' | 'monthly';
export type Theme = 'light' | 'dark';

export interface CreateTaskRequest {
    title: string;
    description?: string;
    due_date?: string;
    priority: TaskPriority;
    tags?: string[];
    recurrence?: TaskRecurrence;
}

export interface UpdateTaskRequest extends Partial<CreateTaskRequest> {
    id?: string;
}

export interface AuthRequest {
    email: string;
    password: string;
}

export interface AuthResponse {
    user: User;
    token?: string;
}

export interface ApiError {
    message: string;
    code?: string;
    details?: unknown;
}

export interface ApiResponse<T = unknown> {
    data?: T;
    error?: ApiError;
    success: boolean;
}
