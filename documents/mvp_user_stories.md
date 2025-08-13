# User Authentication - Email Signup

Tags: "authentication,backend,frontend"

You are to implement a user authentication system for the MVP of a To Do web application.

Technical Requirements:

-   Authentication type: Email & password.
-   Email verification: Not required for MVP.
-   Backend: .NET 8 Web API with REST endpoints.
-   Database: PostgreSQL table 'users' with fields: id (UUID, PK), email (unique, indexed), password_hash, created_at, updated_at.
-   Password hashing: bcrypt with a work factor of 12.
-   Validation: Email format check, password minimum length 8 characters.
-   Error handling: Return HTTP 400 for invalid input, HTTP 409 for duplicate email.
-   Frontend: Next.js with responsive form for signup, field validation, and error display.

Definition of Done:

-   User can register with email and password.
-   Passwords are hashed before being stored in the database.
-   Duplicate email attempts return error without creating new account.
-   Frontend form prevents submission with invalid inputs.
-   API tests confirm registration works and data is stored correctly."

# User Authentication - Email Login

Tags: "authentication,backend,frontend"

You are to implement a login system for the MVP of a To Do web application.

Technical Requirements:

-   Authentication type: Email & password.
-   Backend: .NET 8 Web API with REST endpoint POST /auth/login.
-   Database: Verify credentials against 'users' table in PostgreSQL.
-   Token management: Return JWT token with 24-hour expiry upon successful login.
-   Security: Use secure JWT signing key stored in environment variables.
-   Validation: Email format check, password minimum length.
-   Error handling: HTTP 401 for invalid credentials.
-   Frontend: Next.js with responsive login form, error messaging, and token storage in secure HTTP-only cookies.

Definition of Done:

-   User can log in with valid credentials and receive a JWT.
-   JWT is stored securely for session persistence.
-   Invalid credentials produce an error message without token creation.
-   API and integration tests confirm authentication flow works end-to-end.

# Task Management - Create Task

Tags: "tasks,backend,frontend"

You are to implement the ability for a user to create a task in the To Do web application.

Technical Requirements:

-   Task fields: id (UUID, PK), user_id (FK), title (string, required), description (text, optional), due_date (timestamp, optional), priority (enum: low, medium, high, required), tags (array of strings, optional), recurrence (enum: none, daily, weekly, monthly, optional), created_at, updated_at.
-   Backend: .NET 8 Web API endpoint POST /tasks.
-   Database: PostgreSQL table 'tasks' with foreign key to 'users' table.
-   Validation: Title max 255 chars, priority must be valid enum value.
-   Security: Authenticated users only (JWT validation middleware).
-   Frontend: Next.js form for creating tasks, responsive UI.

Definition of Done:

-   Authenticated user can create a task with all supported fields.
-   Task is stored in the database linked to the user.
-   API validates inputs and rejects invalid data.
-   Frontend confirms task creation visually without page reload.
-   Unit tests verify task creation API logic.

# Task Management - Edit Task

Tags: "tasks,backend,frontend"
You are to implement the ability for a user to edit an existing task.

Technical Requirements:

-   Editable fields: title, description, due_date, priority, tags, recurrence.
-   Backend: .NET 8 Web API endpoint PUT /tasks/{task_id}.
-   Database: Update task in 'tasks' table, preserving created_at and updating updated_at.
-   Security: Authenticated users only. Ensure task belongs to logged-in user.
-   Validation: Same rules as task creation.
-   Frontend: Next.js form pre-filled with task data.

Definition of Done:

-   User can update all editable fields for their own tasks.
-   Updated data is persisted in the database.
-   Unauthorized access (editing another user’s task) returns HTTP 403.
-   API and frontend validations are in place.
-   Integration tests confirm data integrity after edit.

# Task Management - Delete Task

Tags: "tasks,backend,frontend"
You are to implement soft deletion for tasks.

Technical Requirements:

-   Backend: .NET 8 Web API endpoint DELETE /tasks/{task_id}.
-   Database: Add boolean 'is_deleted' column to 'tasks' table. Update this instead of removing row.
-   Security: Authenticated users only. Ensure task belongs to logged-in user.
-   Frontend: Next.js confirmation modal before deletion.
-   API: Return HTTP 200 with success message after soft-delete.

Definition of Done:

-   User can delete their own tasks.
-   Deleted tasks are flagged in DB and excluded from default task queries.
-   Attempting to delete another user’s task returns HTTP 403.
-   API tests verify soft delete logic and filtering works.

# UI/UX - Light/Dark Theme Toggle

Tags: "frontend,ui-ux"
You are to implement a theme toggle feature.

Technical Requirements:

-   Frontend: Next.js with Tailwind CSS theme variables.
-   Store theme preference in browser localStorage.
-   Apply theme globally across all pages and components.
-   Toggle should be accessible from the main navigation bar.

Definition of Done:

-   User can toggle between light and dark mode.
-   Preference is retained on page reload.
-   All components respond visually to theme change.
-   UI tests confirm correct theme persistence.

# Accessibility Compliance

Tags: "frontend,ui-ux,accessibility"
You are to ensure the application meets WCAG AA accessibility standards.

Technical Requirements:

-   Contrast ratio: Minimum 4.5:1 for text and UI elements.
-   ARIA labels for all interactive elements.
-   Keyboard navigation support for all UI components.
-   Screen reader support verified with NVDA or VoiceOver.

Definition of Done:

-   Accessibility audit passes WCAG AA criteria.
-   Interactive elements are usable with keyboard only.
-   Screen readers correctly announce UI elements.
-   Automated accessibility tests pass in CI/CD.
