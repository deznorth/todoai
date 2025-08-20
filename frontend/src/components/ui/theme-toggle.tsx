'use client';

import React, { useEffect, useState } from 'react';
import { Moon, Sun } from 'lucide-react';
import { Button } from './button';
import { useTheme } from '@/hooks/use-theme';

export const ThemeToggle: React.FC = () => {
    const [mounted, setMounted] = useState(false);
    const { theme, toggleTheme } = useTheme();

    // Only render after mounting to avoid hydration mismatch
    useEffect(() => {
        setMounted(true);
    }, []);

    if (!mounted) {
        // Return a placeholder that matches the final button size
        return (
            <Button variant="ghost" size="sm" className="h-9 w-9 rounded-md border border-input bg-background" disabled>
                <Moon className="h-4 w-4" />
            </Button>
        );
    }

    return (
        <Button
            variant="ghost"
            size="sm"
            onClick={toggleTheme}
            aria-label={`Switch to ${theme === 'light' ? 'dark' : 'light'} theme`}
            className="h-9 w-9 rounded-md border border-input bg-background hover:bg-accent hover:text-accent-foreground transition-all duration-200"
            type="button"
        >
            {theme === 'dark' ? (
                <Moon className="h-4 w-4 rotate-0 scale-100 transition-all" data-testid="moon-icon" />
            ) : (
                <Sun className="h-4 w-4 rotate-0 scale-100 transition-all" data-testid="sun-icon" />
            )}
            <span className="sr-only">
                Toggle theme from {theme} to {theme === 'light' ? 'dark' : 'light'}
            </span>
        </Button>
    );
};
