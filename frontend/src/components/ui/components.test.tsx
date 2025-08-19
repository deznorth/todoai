import { render, screen } from '@testing-library/react';
import { Button } from './button';
import {
    Card,
    CardContent,
    CardDescription,
    CardFooter,
    CardHeader,
    CardTitle,
} from './card';
import { Input } from './input';
import { Label } from './label';

describe('shadcn/ui Components', () => {
    describe('Button Component', () => {
        it('renders button with text', () => {
            render(<Button>Click me</Button>);

            const button = screen.getByRole('button', { name: /click me/i });
            expect(button).toBeInTheDocument();
        });

        it('renders button with variant prop', () => {
            render(<Button variant="outline">Outline Button</Button>);

            const button = screen.getByRole('button', {
                name: /outline button/i,
            });
            expect(button).toBeInTheDocument();
            expect(button).toHaveClass('border');
        });
    });

    describe('Card Component', () => {
        it('renders card with all sub-components', () => {
            render(
                <Card>
                    <CardHeader>
                        <CardTitle>Card Title</CardTitle>
                        <CardDescription>Card Description</CardDescription>
                    </CardHeader>
                    <CardContent>
                        <p>Card content goes here</p>
                    </CardContent>
                    <CardFooter>
                        <Button>Action</Button>
                    </CardFooter>
                </Card>
            );

            expect(screen.getByText('Card Title')).toBeInTheDocument();
            expect(screen.getByText('Card Description')).toBeInTheDocument();
            expect(
                screen.getByText('Card content goes here')
            ).toBeInTheDocument();
            expect(
                screen.getByRole('button', { name: /action/i })
            ).toBeInTheDocument();
        });
    });

    describe('Input Component', () => {
        it('renders input field', () => {
            render(<Input placeholder="Enter text" />);

            const input = screen.getByPlaceholderText('Enter text');
            expect(input).toBeInTheDocument();
            expect(input.tagName).toBe('INPUT');
        });

        it('renders input with different types', () => {
            render(<Input type="email" placeholder="Enter email" />);

            const input = screen.getByPlaceholderText('Enter email');
            expect(input).toHaveAttribute('type', 'email');
        });
    });

    describe('Label Component', () => {
        it('renders label with text', () => {
            render(<Label htmlFor="test-input">Test Label</Label>);

            const label = screen.getByText('Test Label');
            expect(label).toBeInTheDocument();
            expect(label).toHaveAttribute('for', 'test-input');
        });
    });

    describe('Form Components Integration', () => {
        it('renders label and input together', () => {
            render(
                <div>
                    <Label htmlFor="username">Username</Label>
                    <Input id="username" placeholder="Enter username" />
                </div>
            );

            const label = screen.getByText('Username');
            const input = screen.getByPlaceholderText('Enter username');

            expect(label).toBeInTheDocument();
            expect(input).toBeInTheDocument();
            expect(label).toHaveAttribute('for', 'username');
            expect(input).toHaveAttribute('id', 'username');
        });
    });
});
