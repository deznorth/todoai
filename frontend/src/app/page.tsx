import { ThemeToggle } from '@/components';

export default function Home() {
    return (
        <div className="min-h-screen bg-background text-foreground">
            <div className="absolute top-4 right-4">
                <ThemeToggle />
            </div>
            <div className="flex items-center justify-center min-h-screen">
                <div className="text-center">
                    <h1 className="text-4xl font-bold mb-4">TodoAI</h1>
                    <p className="text-muted-foreground mb-6">Frontend boilerplate ready for development</p>
                    <p className="text-sm text-muted-foreground">
                        Click the theme toggle in the top-right corner to test dark/light mode
                    </p>
                </div>
            </div>
        </div>
    );
}
