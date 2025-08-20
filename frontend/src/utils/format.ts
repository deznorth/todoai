// Formatting utility functions

import type { TaskPriority } from '@/types';

export const formatTaskPriority = (priority: TaskPriority): string => {
    const priorityMap = {
        low: 'Low Priority',
        medium: 'Medium Priority',
        high: 'High Priority',
    };
    return priorityMap[priority];
};

export const getPriorityColor = (priority: TaskPriority): string => {
    const colorMap = {
        low: 'text-green-600 dark:text-green-400',
        medium: 'text-yellow-600 dark:text-yellow-400',
        high: 'text-red-600 dark:text-red-400',
    };
    return colorMap[priority];
};

export const getPriorityBadgeColor = (priority: TaskPriority): string => {
    const colorMap = {
        low: 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200',
        medium: 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200',
        high: 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200',
    };
    return colorMap[priority];
};

export const truncateText = (text: string, maxLength: number = 100): string => {
    if (text.length <= maxLength) return text;
    return text.slice(0, maxLength).trim() + '...';
};

export const formatTagList = (tags: string[]): string => {
    if (tags.length === 0) return 'No tags';
    if (tags.length === 1) {
        const firstTag = tags[0];
        return firstTag || '';
    }
    if (tags.length === 2) return tags.join(' and ');
    const lastTag = tags[tags.length - 1];
    return `${tags.slice(0, -1).join(', ')} and ${lastTag || ''}`;
};

export const capitalizeFirst = (text: string): string => {
    return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase();
};

export const formatRecurrence = (recurrence: string): string => {
    const recurrenceMap = {
        none: 'No recurrence',
        daily: 'Daily',
        weekly: 'Weekly',
        monthly: 'Monthly',
    };
    return recurrenceMap[recurrence as keyof typeof recurrenceMap] || recurrence;
};