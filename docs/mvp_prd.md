# Product Requirements Document (PRD) – MVP: To Do Web Application

## 1. Product Overview
A minimalist, web-based task management application for general consumers and students. 
The app enables users to create, organize, and track personal tasks with **due dates**, **priorities**, **tags**, and **recurring schedules**. 
It features a clean and aesthetic interface with **light and dark themes** and is **accessible** to users with visual or mobility impairments.

---

## 2. Goals & Success Metrics

### Goals
- Deliver a **personal task manager** that is easy to use and visually appealing.
- Support **web-only MVP**, but design for future cross-platform expansion.
- Establish a **scalable backend architecture** for future integrations.

### Success Metrics
- Average of **5+ tasks created per active user** within first week.
- **30% retention rate** after 4 weeks.
- **60%+ task completion rate** for engaged users.

---

## 3. Target Audience
- **Primary**: Students and general consumers seeking a personal productivity tool.
- **Secondary**: Household/family users who might use shared tasks in future versions.

---

## 4. Core MVP Features

### 4.1 User Authentication
- **Type**: Email and password authentication.
- **Verification**: No email verification for MVP.
- **Security**: Passwords hashed with bcrypt (work factor 12).
- **Persistence**: JWT tokens for authentication, stored in secure HTTP-only cookies.

### 4.2 Task Management
- **CRUD Operations**: Create, Read, Update, Delete (soft delete).
- **Task Fields**:
  - `id` (UUID, PK)
  - `user_id` (FK to users)
  - `title` (string, required, max 255 chars)
  - `description` (text, optional)
  - `due_date` (timestamp, optional)
  - `priority` (enum: low, medium, high, required)
  - `tags` (array of strings, optional)
  - `recurrence` (enum: none, daily, weekly, monthly, optional)
  - `is_deleted` (boolean, default false)
  - `created_at` and `updated_at` (timestamps)
- **Validation**:
  - Title is required, max 255 chars.
  - Priority must be valid enum value.
- **Security**: Only the task owner can view/edit/delete.

### 4.3 UI/UX
- **Theme Toggle**: Light and dark mode with persistence in `localStorage`.
- **Responsive Design**: Works on desktop and mobile browsers.
- **Accessibility**: WCAG AA compliance, ARIA labels, keyboard navigation, screen reader support.

---

## 5. Non-Functional Requirements
- **Performance**: Page load under 2s on standard broadband.
- **Scalability**: Backend ready for future integrations (e.g., calendar sync).
- **Security**: Follows OWASP best practices.
- **Reliability**: 99.9% uptime SLA.

---

## 6. Technical Stack
- **Frontend**: Next.js (React), Tailwind CSS for styling.
- **Backend**: .NET 8 Web API.
- **Database**: PostgreSQL.
- **Hosting**: Vercel (frontend), Azure or AWS (backend & database).
- **Auth**: JWT-based authentication.

---

## 7. API Endpoints (MVP Scope)

### User Authentication
- `POST /auth/signup`  
  Registers a new user with email and password.
- `POST /auth/login`  
  Authenticates and returns JWT token.

### Tasks
- `GET /tasks`  
  Returns all non-deleted tasks for the authenticated user.
- `POST /tasks`  
  Creates a new task.
- `PUT /tasks/{task_id}`  
  Updates an existing task (must belong to authenticated user).
- `DELETE /tasks/{task_id}`  
  Soft-deletes a task by setting `is_deleted = true`.

---

## 8. Acceptance Criteria Summary

### Authentication
- Users can register and log in using email/password.
- Passwords stored using bcrypt.
- JWT token is returned and stored in HTTP-only cookie.

### Task Management
- Authenticated users can CRUD their own tasks.
- Validation errors are returned with clear messages.
- Soft-deleted tasks are not shown in default queries.

### UI/UX
- Theme toggle works and persists preference.
- Fully responsive on mobile and desktop.
- Passes WCAG AA accessibility checks.

---

## 9. MVP Timeline

| Phase                | Duration  | Deliverables |
|----------------------|-----------|--------------|
| Discovery & Design   | 2 weeks   | Wireframes, final PRD |
| Sprint 1             | 3 weeks   | Auth system, basic CRUD |
| Sprint 2             | 3 weeks   | Due dates, priorities, tags, recurrence |
| QA & UAT             | 2 weeks   | Bug fixes, accessibility checks |
| Launch               | 1 week    | Production release & monitoring |

---

## 10. Future Features (Post-MVP)
- Mobile apps for iOS and Android.
- Shared task lists for household use.
- Calendar integrations.
- Push notifications and reminders.
