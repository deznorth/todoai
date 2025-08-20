# TodoAI Frontend

Modern, responsive frontend for the TodoAI task management application built with Next.js 15, TypeScript, and Tailwind CSS.

## Features

- ⚡ **Next.js 15** with App Router and Turbopack
- 🎨 **Tailwind CSS** with custom theme system
- 🌓 **Dark/Light Mode** with persistent theme toggle
- 🧩 **shadcn/ui** component library
- 📱 **Responsive Design** across all devices
- ♿ **WCAG AA Accessibility** compliance
- 🧪 **Comprehensive Test Suite** with Jest and React Testing Library
- 📝 **TypeScript** with strict configuration
- 🔧 **ESLint & Prettier** for code quality

## Tech Stack

- **Framework**: Next.js 15+ with App Router
- **Language**: TypeScript
- **Styling**: Tailwind CSS with custom theme variables
- **Components**: shadcn/ui (Radix primitives)
- **Icons**: Lucide React
- **Testing**: Jest + React Testing Library
- **Linting**: ESLint + Prettier

## Getting Started

### Prerequisites

- Node.js 18+ 
- Yarn (recommended) or npm

### Installation

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd todoai/frontend
   ```

2. **Install dependencies**:
   ```bash
   yarn install
   ```

3. **Set up environment variables**:
   ```bash
   cp .env.example .env.local
   # Edit .env.local with your API endpoints
   ```

### Development

1. **Start the development server**:
   ```bash
   yarn dev
   ```

2. **Open your browser** and navigate to [http://localhost:3000](http://localhost:3000)

3. **Start building!** The page auto-updates as you edit files.

### Available Scripts

- `yarn dev` - Start development server with Turbopack
- `yarn build` - Build production application
- `yarn start` - Start production server
- `yarn test` - Run test suite
- `yarn test:watch` - Run tests in watch mode
- `yarn lint` - Run ESLint
- `yarn lint:fix` - Fix ESLint issues
- `yarn type-check` - Run TypeScript type checking

## Project Structure

```
src/
├── app/                 # Next.js App Router pages
├── components/          # Reusable UI components
│   ├── ui/             # shadcn/ui base components
│   └── layout/         # Layout components
├── lib/                # Utility functions
├── providers/          # React context providers
├── hooks/              # Custom React hooks
├── types/              # TypeScript type definitions
├── constants/          # Application constants
└── styles/             # Global styles and themes
```

## Theme System

The application features a robust theme system with:

- **Dark mode as default** (as per project requirements)
- **Light mode toggle** available
- **Persistent theme preference** stored in localStorage
- **System preference detection** with manual override
- **Smooth transitions** between themes
- **WCAG AA compliant** color contrast ratios

### Using Themes

```tsx
import { useTheme } from '@/hooks/use-theme';

function MyComponent() {
  const { theme, toggleTheme, setTheme } = useTheme();
  
  return (
    <button onClick={toggleTheme}>
      Current theme: {theme}
    </button>
  );
}
```

## Component Library

Built with shadcn/ui components for consistent design:

- **Button** - Multiple variants and sizes
- **Card** - Content containers
- **Input** - Form inputs with validation
- **Label** - Accessible form labels
- **Theme Toggle** - Dark/light mode switcher

### Adding New Components

```bash
# Install a new shadcn/ui component
npx shadcn@latest add <component-name>
```

## Testing

Comprehensive test suite covering:

- **Unit tests** for components and utilities
- **Integration tests** for user interactions
- **Accessibility tests** for WCAG compliance
- **Theme tests** for dark/light mode functionality

### Running Tests

```bash
# Run all tests
yarn test

# Run tests in watch mode
yarn test:watch

# Run specific test file
yarn test components/ui/theme-toggle.test.tsx

# Run tests with coverage
yarn test --coverage
```

## Code Quality

### ESLint Configuration

- Next.js recommended rules
- TypeScript strict rules
- React hooks rules
- Accessibility rules

### Prettier Configuration

- Consistent code formatting
- Automatic import organization
- Tailwind CSS class sorting

## Environment Variables

Create `.env.local` with:

```env
NEXT_PUBLIC_API_URL=http://localhost:5000
```

## API Integration

The frontend is configured to connect with the .NET 8 Web API backend:

- **Base URL**: `http://localhost:5000` (development)
- **Authentication**: JWT tokens in HTTP-only cookies
- **Endpoints**: `/auth/*` and `/tasks/*`

## Accessibility

WCAG AA compliance features:

- **Keyboard navigation** for all interactive elements
- **Screen reader support** with ARIA labels
- **Color contrast** ratios meeting AA standards
- **Focus management** with visible focus indicators
- **Responsive touch targets** (minimum 44px)

## Performance

Optimizations included:

- **Turbopack** for fast development builds
- **Next.js Image** component for optimized images
- **Font optimization** with next/font
- **Tree shaking** for minimal bundle size
- **Code splitting** with dynamic imports

## Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## Contributing

1. Follow the existing code style
2. Write tests for new features
3. Ensure accessibility compliance
4. Update documentation as needed

## Learn More

- [Next.js Documentation](https://nextjs.org/docs)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
- [shadcn/ui Documentation](https://ui.shadcn.com)
- [React Testing Library](https://testing-library.com/docs/react-testing-library/intro)
