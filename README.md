# ToDoAI

This is a project to test what working alongside an AI for every step of the SDLC would be like. Using AI to fulfill as many of the team's roles as possible

## Development Status

### 🔄 In Progress

-   None currently

### ❌ Pending Implementation

**Frontend (Next.js):**

-   Authentication pages (login/signup forms)
-   Task management interface (list, create, edit, delete)
-   Dark/light theme toggle with localStorage
-   Responsive design for desktop and mobile
-   WCAG AA accessibility compliance
-   API integration with backend

**Deployment:**

-   Frontend deployment to Vercel (TBD, this was the AI's suggestion)
-   Backend deployment to Azure/AWS (TBD, this was the AI's suggestion)
-   Production database hosting
-   Environment configuration for production

### ✅ Completed Components

<details>
<summary>Day 1 - 8/12/25 - Planning Day</summary>

-   Project plan and setup
-   Got access to Claude AI and set it up to work with my work environment
</details>
<details>
<summary>Day 2 - 8/13/25 - UI research and fooling around day</summary>

-   Learned about and set up Agent OS
-   Researched UI desig AIs a bit
-   Messed around with Figma Make
</details>
<details open>
<summary>Day 3 - 8/18/25 - Full back-end with unit and integration tests</summary>

**Backend (.NET 8 Web API):**

-   Complete TodoApi project with all endpoints implemented
-   Entity Framework Core with PostgreSQL integration
-   JWT authentication with HTTP-only cookies
-   BCrypt password hashing (work factor 12)
-   Input validation and error handling
-   User authorization (users can only access their own tasks)
-   Soft delete functionality for tasks
-   Database migrations configured and applied

**Database (PostgreSQL):**

-   Users and Tasks tables implemented with proper relationships
-   Database schema matches specifications exactly
-   Docker Compose configuration for local development
-   EF Core migrations set up and working

**Testing Infrastructure:**

-   Comprehensive test suite with 51 passing tests
-   Unit tests for AuthService and TaskService (17 tests)
-   Integration tests for AuthController and TasksController (34 tests)
-   Custom TestWebApplicationFactory for proper test isolation
-   Fluent Assertions for readable test code
-   Test configuration with appsettings.json
</details>
