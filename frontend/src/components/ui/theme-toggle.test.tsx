/**
 * Theme Toggle Component Tests
 * Tests for the theme toggle button component with sun/moon icons
 */

import { render, screen, fireEvent } from '@testing-library/react';
import { ThemeToggle } from './theme-toggle';
import { ThemeProvider } from '@/providers/theme-provider';

// Mock localStorage
const localStorageMock = {
    getItem: jest.fn(),
    setItem: jest.fn(),
    clear: jest.fn(),
};
Object.defineProperty(window, 'localStorage', {
    value: localStorageMock,
});

describe('ThemeToggle Component', () => {
    beforeEach(() => {
        localStorageMock.getItem.mockClear();
        localStorageMock.setItem.mockClear();
        document.documentElement.className = '';
    });

    const renderThemeToggle = (initialTheme = 'dark') => {
        localStorageMock.getItem.mockReturnValue(initialTheme);

        return render(
            <ThemeProvider>
                <ThemeToggle />
            </ThemeProvider>
        );
    };

    describe('Rendering', () => {
        it('should render toggle button', () => {
            renderThemeToggle();

            const button = screen.getByRole('button', { name: /switch to/i });
            expect(button).toBeInTheDocument();
        });

        it('should show moon icon when dark theme is active', () => {
            renderThemeToggle('dark');

            const moonIcon = screen.getByTestId('moon-icon');
            expect(moonIcon).toBeInTheDocument();
        });

        it('should show sun icon when light theme is active', () => {
            renderThemeToggle('light');

            const sunIcon = screen.getByTestId('sun-icon');
            expect(sunIcon).toBeInTheDocument();
        });

        it('should have proper accessibility attributes', () => {
            renderThemeToggle();

            const button = screen.getByRole('button', { name: /switch to/i });
            expect(button).toHaveAttribute('aria-label');
            expect(button).toHaveAttribute('type', 'button');
        });
    });

    describe('Theme Switching', () => {
        it('should toggle theme when clicked', () => {
            renderThemeToggle('dark');

            const button = screen.getByRole('button', { name: /switch to/i });

            // Should start with moon icon (dark theme)
            expect(screen.getByTestId('moon-icon')).toBeInTheDocument();

            // Click to toggle to light
            fireEvent.click(button);

            // Should show sun icon (light theme)
            expect(screen.getByTestId('sun-icon')).toBeInTheDocument();
            expect(screen.queryByTestId('moon-icon')).not.toBeInTheDocument();
        });

        it('should toggle from light to dark', () => {
            renderThemeToggle('light');

            const button = screen.getByRole('button', { name: /switch to/i });

            // Should start with sun icon (light theme)
            expect(screen.getByTestId('sun-icon')).toBeInTheDocument();

            // Click to toggle to dark
            fireEvent.click(button);

            // Should show moon icon (dark theme)
            expect(screen.getByTestId('moon-icon')).toBeInTheDocument();
            expect(screen.queryByTestId('sun-icon')).not.toBeInTheDocument();
        });

        it('should persist theme changes to localStorage', () => {
            renderThemeToggle('dark');

            const button = screen.getByRole('button', { name: /switch to/i });
            fireEvent.click(button);

            expect(localStorageMock.setItem).toHaveBeenCalledWith('todoai-theme', 'light');
        });
    });

    describe('Icon Animation', () => {
        it('should have transition classes for smooth icon changes', () => {
            renderThemeToggle('dark');

            const button = screen.getByRole('button');

            // Button should have transition classes
            expect(button).toHaveClass('transition-all');
        });

        it('should apply hover effects', () => {
            renderThemeToggle();

            const button = screen.getByRole('button');

            // Should have hover classes
            expect(button).toHaveClass('hover:bg-accent');
        });
    });

    describe('Responsive Design', () => {
        it('should have appropriate sizing classes', () => {
            renderThemeToggle();

            const button = screen.getByRole('button');

            // Should have proper size classes
            expect(button).toHaveClass('h-9', 'w-9');
        });

        it('should have proper icon sizing', () => {
            renderThemeToggle('dark');

            const moonIcon = screen.getByTestId('moon-icon');

            // Icon should have proper size
            expect(moonIcon).toHaveClass('h-4', 'w-4');
        });
    });
});
