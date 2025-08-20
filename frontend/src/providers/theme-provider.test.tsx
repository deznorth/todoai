/**
 * Theme Provider Tests
 * Tests for theme context provider and theme switching functionality
 */

import { render, screen, fireEvent } from '@testing-library/react';
import { ThemeProvider } from './theme-provider';
import { useTheme } from '@/hooks/use-theme';

// Mock localStorage
const localStorageMock = {
    getItem: jest.fn(),
    setItem: jest.fn(),
    clear: jest.fn(),
};
Object.defineProperty(window, 'localStorage', {
    value: localStorageMock,
});

// Test component to interact with theme context
const TestComponent = () => {
    const { theme, setTheme, toggleTheme } = useTheme();

    return (
        <div>
            <span data-testid="current-theme">{theme}</span>
            <button onClick={() => setTheme('light')} data-testid="set-light">
                Set Light
            </button>
            <button onClick={() => setTheme('dark')} data-testid="set-dark">
                Set Dark
            </button>
            <button onClick={toggleTheme} data-testid="toggle">
                Toggle
            </button>
        </div>
    );
};

describe('ThemeProvider', () => {
    beforeEach(() => {
        localStorageMock.getItem.mockClear();
        localStorageMock.setItem.mockClear();
        localStorageMock.clear.mockClear();
        // Reset DOM classes
        document.documentElement.className = '';
    });

    describe('Theme Context', () => {
        it('should provide default dark theme', () => {
            localStorageMock.getItem.mockReturnValue(null);

            render(
                <ThemeProvider>
                    <TestComponent />
                </ThemeProvider>
            );

            expect(screen.getByTestId('current-theme')).toHaveTextContent('dark');
        });

        it('should load theme from localStorage', () => {
            localStorageMock.getItem.mockReturnValue('light');

            render(
                <ThemeProvider>
                    <TestComponent />
                </ThemeProvider>
            );

            expect(screen.getByTestId('current-theme')).toHaveTextContent('light');
        });

        it('should handle invalid localStorage values', () => {
            localStorageMock.getItem.mockReturnValue('invalid-theme');

            render(
                <ThemeProvider>
                    <TestComponent />
                </ThemeProvider>
            );

            expect(screen.getByTestId('current-theme')).toHaveTextContent('dark');
        });
    });

    describe('Theme Switching', () => {
        it('should change theme when setTheme is called', () => {
            localStorageMock.getItem.mockReturnValue(null);

            render(
                <ThemeProvider>
                    <TestComponent />
                </ThemeProvider>
            );

            fireEvent.click(screen.getByTestId('set-light'));
            expect(screen.getByTestId('current-theme')).toHaveTextContent('light');

            fireEvent.click(screen.getByTestId('set-dark'));
            expect(screen.getByTestId('current-theme')).toHaveTextContent('dark');
        });

        it('should toggle between light and dark themes', () => {
            localStorageMock.getItem.mockReturnValue(null);

            render(
                <ThemeProvider>
                    <TestComponent />
                </ThemeProvider>
            );

            // Start with dark (default)
            expect(screen.getByTestId('current-theme')).toHaveTextContent('dark');

            // Toggle to light
            fireEvent.click(screen.getByTestId('toggle'));
            expect(screen.getByTestId('current-theme')).toHaveTextContent('light');

            // Toggle back to dark
            fireEvent.click(screen.getByTestId('toggle'));
            expect(screen.getByTestId('current-theme')).toHaveTextContent('dark');
        });
    });

    describe('LocalStorage Persistence', () => {
        it('should save theme to localStorage when changed', () => {
            localStorageMock.getItem.mockReturnValue(null);

            render(
                <ThemeProvider>
                    <TestComponent />
                </ThemeProvider>
            );

            fireEvent.click(screen.getByTestId('set-light'));

            expect(localStorageMock.setItem).toHaveBeenCalledWith('todoai-theme', 'light');
        });

        it('should save theme when toggled', () => {
            localStorageMock.getItem.mockReturnValue('dark');

            render(
                <ThemeProvider>
                    <TestComponent />
                </ThemeProvider>
            );

            fireEvent.click(screen.getByTestId('toggle'));

            expect(localStorageMock.setItem).toHaveBeenCalledWith('todoai-theme', 'light');
        });
    });

    describe('DOM Class Management', () => {
        it('should apply light class to documentElement when theme is light', () => {
            localStorageMock.getItem.mockReturnValue('light');

            render(
                <ThemeProvider>
                    <TestComponent />
                </ThemeProvider>
            );

            expect(document.documentElement.classList.contains('light')).toBe(true);
        });

        it('should not apply light class when theme is dark', () => {
            localStorageMock.getItem.mockReturnValue('dark');

            render(
                <ThemeProvider>
                    <TestComponent />
                </ThemeProvider>
            );

            expect(document.documentElement.classList.contains('light')).toBe(false);
        });

        it('should update DOM class when theme changes', () => {
            localStorageMock.getItem.mockReturnValue('dark');

            render(
                <ThemeProvider>
                    <TestComponent />
                </ThemeProvider>
            );

            // Should start without light class
            expect(document.documentElement.classList.contains('light')).toBe(false);

            // Change to light theme
            fireEvent.click(screen.getByTestId('set-light'));
            expect(document.documentElement.classList.contains('light')).toBe(true);

            // Change back to dark theme
            fireEvent.click(screen.getByTestId('set-dark'));
            expect(document.documentElement.classList.contains('light')).toBe(false);
        });
    });
});
