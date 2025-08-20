'use client';

import React, { createContext, useContext, useEffect, useState } from 'react';
import type { Theme } from '@/types';
import { THEME_CONFIG } from '@/constants';

interface ThemeContextType {
    theme: Theme;
    setTheme: (theme: Theme) => void;
    toggleTheme: () => void;
}

const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

interface ThemeProviderProps {
    children: React.ReactNode;
    defaultTheme?: Theme;
    storageKey?: string;
}

export const ThemeProvider: React.FC<ThemeProviderProps> = ({
    children,
    defaultTheme = THEME_CONFIG.defaultTheme as Theme,
    storageKey = THEME_CONFIG.storageKey,
}) => {
    const [theme, setThemeState] = useState<Theme>(defaultTheme);
    const [mounted, setMounted] = useState(false);

    // Load theme from localStorage on mount
    useEffect(() => {
        try {
            const savedTheme = localStorage.getItem(storageKey);
            if (savedTheme && (savedTheme === 'light' || savedTheme === 'dark')) {
                setThemeState(savedTheme as Theme);
            }
        } catch (error) {
            // Handle localStorage access errors (e.g., SSR, private browsing)
            if (process.env.NODE_ENV === 'development') {
                // eslint-disable-next-line no-console
                console.warn('Failed to load theme from localStorage:', error);
            }
        }
        setMounted(true);
    }, [storageKey]);

    // Apply theme to document element
    useEffect(() => {
        if (!mounted) return;

        const root = document.documentElement;

        // Remove existing theme classes
        root.classList.remove('light', 'dark');

        // Apply theme class
        if (theme === 'light') {
            root.classList.add('light');
        }
        // For dark theme, we don't add a class since it's the default in CSS
    }, [theme, mounted]);

    const setTheme = (newTheme: Theme) => {
        try {
            localStorage.setItem(storageKey, newTheme);
        } catch (error) {
            // Handle localStorage access errors
            if (process.env.NODE_ENV === 'development') {
                // eslint-disable-next-line no-console
                console.warn('Failed to save theme to localStorage:', error);
            }
        }
        setThemeState(newTheme);
    };

    const toggleTheme = () => {
        setTheme(theme === 'light' ? 'dark' : 'light');
    };

    const value: ThemeContextType = {
        theme,
        setTheme,
        toggleTheme,
    };

    // Don't render anything until mounted to avoid hydration mismatch
    if (!mounted) {
        return <>{children}</>;
    }

    return <ThemeContext.Provider value={value}>{children}</ThemeContext.Provider>;
};

export const useThemeContext = (): ThemeContextType => {
    const context = useContext(ThemeContext);
    if (context === undefined) {
        throw new Error('useThemeContext must be used within a ThemeProvider');
    }
    return context;
};
