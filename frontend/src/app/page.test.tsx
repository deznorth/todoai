import { render, screen } from '@testing-library/react';
import Home from './page';
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

describe('Home page', () => {
    beforeEach(() => {
        localStorageMock.getItem.mockClear();
        localStorageMock.setItem.mockClear();
        localStorageMock.getItem.mockReturnValue(null);
        document.documentElement.className = '';
    });

    const renderWithTheme = () => {
        return render(
            <ThemeProvider>
                <Home />
            </ThemeProvider>
        );
    };

    it('renders the TodoAI title', () => {
        renderWithTheme();

        const heading = screen.getByRole('heading', { name: /todoai/i });
        expect(heading).toBeInTheDocument();
    });

    it('renders the boilerplate ready message', () => {
        renderWithTheme();

        const message = screen.getByText(/frontend boilerplate ready for development/i);
        expect(message).toBeInTheDocument();
    });

    it('renders the theme toggle button', () => {
        renderWithTheme();

        const themeToggle = screen.getByRole('button', { name: /switch to light theme/i });
        expect(themeToggle).toBeInTheDocument();
    });
});
