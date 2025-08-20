// Validation utility functions

export const isValidEmail = (email: string): boolean => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
};

export const isValidPassword = (password: string): boolean => {
    // At least 8 characters, 1 uppercase, 1 lowercase, 1 number
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/;
    return passwordRegex.test(password);
};

export const isValidTaskTitle = (title: string): boolean => {
    return title.trim().length > 0 && title.length <= 255;
};

export const isValidDueDate = (date: string): boolean => {
    const parsedDate = new Date(date);
    return !isNaN(parsedDate.getTime()) && parsedDate >= new Date(new Date().setHours(0, 0, 0, 0));
};

export const validateRequired = (value: string, fieldName: string): string | null => {
    if (!value.trim()) {
        return `${fieldName} is required`;
    }
    return null;
};

export const validateEmail = (email: string): string | null => {
    if (!email.trim()) return 'Email is required';
    if (!isValidEmail(email)) return 'Please enter a valid email address';
    return null;
};

export const validatePassword = (password: string): string | null => {
    if (!password) return 'Password is required';
    if (!isValidPassword(password)) {
        return 'Password must be at least 8 characters with uppercase, lowercase, and number';
    }
    return null;
};

export const validateTaskTitle = (title: string): string | null => {
    if (!title.trim()) return 'Task title is required';
    if (title.length > 255) return 'Task title must be 255 characters or less';
    return null;
};

export const validateDueDate = (date: string): string | null => {
    if (date && !isValidDueDate(date)) {
        return 'Due date must be today or in the future';
    }
    return null;
};