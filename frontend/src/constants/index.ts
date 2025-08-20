// Application constants barrel export

export const APP_CONFIG = {
    name: 'TodoAI',
    version: '1.0.0',
    description: 'AI-driven todo application',
} as const;

export const API_ENDPOINTS = {
    base: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000',
    auth: {
        login: '/auth/login',
        signup: '/auth/signup',
        logout: '/auth/logout',
    },
    tasks: {
        list: '/tasks',
        create: '/tasks',
        update: (id: string) => `/tasks/${id}`,
        delete: (id: string) => `/tasks/${id}`,
    },
} as const;

export const THEME_CONFIG = {
    defaultTheme: 'dark',
    storageKey: 'todoai-theme',
} as const;

export const TASK_PRIORITIES = {
    low: 'low',
    medium: 'medium',
    high: 'high',
} as const;

export const TASK_RECURRENCE = {
    none: 'none',
    daily: 'daily',
    weekly: 'weekly',
    monthly: 'monthly',
} as const;
