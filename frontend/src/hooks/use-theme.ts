// Theme hook using context
import { useThemeContext } from '@/providers/theme-provider';

export const useTheme = () => {
    try {
        return useThemeContext();
    } catch {
        // Return default values if context is not available (e.g., during SSR)
        return {
            theme: 'dark' as const,
            setTheme: () => {},
            toggleTheme: () => {},
        };
    }
};
