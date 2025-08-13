# Technical Stack

> Last Updated: 2025-08-13
> Version: 1.0.0

## Application Framework

- **Frontend Framework:** Next.js (React)
- **Backend Framework:** .NET 8 Web API
- **Architecture Pattern:** Domain Driven Architecture (backend)

## Database

- **Primary Database:** PostgreSQL
- **Migration Strategy:** Database migrations with reverting scripts
- **Schema Management:** Entity Framework Core (backend)

## JavaScript Framework

- **Framework:** Next.js (React-based)
- **State Management:** React hooks and context
- **TypeScript:** Full TypeScript implementation

## CSS Framework

- **Framework:** Tailwind CSS
- **Theme System:** Light/dark mode toggle with localStorage persistence
- **Responsive Design:** Mobile-first approach with full responsive support

## Authentication & Security

- **Authentication Method:** JWT-based authentication
- **Token Storage:** HTTP-only cookies (24-hour expiry)
- **Password Hashing:** bcrypt with work factor 12
- **Authorization:** User-based resource isolation

## Testing Framework

- **Backend Testing:** XUnit framework
- **Frontend Testing:** Jest with React Testing Library
- **Coverage Target:** Comprehensive test coverage across all layers

## Hosting & Deployment

- **Frontend Hosting:** Vercel
- **Backend Hosting:** Azure/AWS (flexible deployment)
- **Database Hosting:** Azure/AWS managed PostgreSQL
- **Performance Target:** Sub-2 second page load times
- **Uptime SLA:** 99.9% availability

## Development Tools

- **Version Control:** Git with conventional commit standards
- **API Documentation:** OpenAPI/Swagger specification
- **Code Quality:** ESLint, Prettier, and .NET analyzers
- **Accessibility:** WCAG AA compliance validation tools

## Architecture Patterns

- **Backend:** Domain Driven Design with clean architecture
- **Frontend:** Component-based architecture with React
- **Data Flow:** RESTful API with JSON communication
- **Error Handling:** Centralized error handling and logging