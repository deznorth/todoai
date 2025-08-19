# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a To Do web application MVP built as an experiment in AI-driven software development lifecycle. The project aims to test working alongside AI for every step of the SDLC, using AI to fulfill as many team roles as possible.

## Architecture

**Tech Stack:**
- Frontend: Next.js (React) with Tailwind CSS
- Backend: .NET 8 Web API  
- Database: PostgreSQL
- Authentication: JWT-based with HTTP-only cookies
- Hosting: Vercel (frontend), Azure/AWS (backend & database)

**Core Features:**
- User authentication (email/password, no verification for MVP)
- Task management with CRUD operations
- Task fields: title, description, due_date, priority (low/medium/high), tags, recurrence (none/daily/weekly/monthly)
- Soft delete functionality (is_deleted flag)
- Light/dark theme toggle with localStorage persistence
- WCAG AA accessibility compliance

## Database Schema

**Users Table:**
- id (UUID, PK)
- email (unique, indexed)
- password_hash (bcrypt, work factor 12)
- created_at, updated_at

**Tasks Table:**
- id (UUID, PK)
- user_id (FK to users)
- title (string, required, max 255 chars)
- description (text, optional)
- due_date (timestamp, optional)
- priority (enum: low, medium, high, required)
- tags (array of strings, optional)
- recurrence (enum: none, daily, weekly, monthly, optional)
- is_deleted (boolean, default false)
- created_at, updated_at

## API Endpoints

**Authentication:**
- POST /auth/signup - Register new user ✅ Implemented
- POST /auth/login - Authenticate and return JWT ✅ Implemented

**Tasks:**
- GET /tasks - Get all non-deleted tasks for authenticated user ✅ Implemented
- GET /tasks/{task_id} - Get specific task by ID (owner only) ✅ Implemented
- POST /tasks - Create new task ✅ Implemented
- PUT /tasks/{task_id} - Update existing task (owner only) ✅ Implemented
- DELETE /tasks/{task_id} - Soft delete task (owner only) ✅ Implemented

## Security Requirements

- Passwords hashed with bcrypt (work factor 12)
- JWT tokens stored in secure HTTP-only cookies (24-hour expiry)
- User authorization: users can only access/modify their own tasks
- Input validation on all endpoints
- OWASP best practices compliance

## Development Status

### ✅ Completed Components

**Backend (.NET 8 Web API):**
- Complete TodoApi project with all endpoints implemented
- Entity Framework Core with PostgreSQL integration
- JWT authentication with HTTP-only cookies
- BCrypt password hashing (work factor 12)
- Input validation and error handling
- User authorization (users can only access their own tasks)
- Soft delete functionality for tasks
- Database migrations configured and applied

**Database (PostgreSQL):**
- Users and Tasks tables implemented with proper relationships
- Database schema matches specifications exactly
- Docker Compose configuration for local development
- EF Core migrations set up and working

**Testing Infrastructure:**
- Comprehensive test suite with 51 passing tests
- Unit tests for AuthService and TaskService (17 tests)
- Integration tests for AuthController and TasksController (34 tests) 
- Custom TestWebApplicationFactory for proper test isolation
- Fluent Assertions for readable test code
- Test configuration with appsettings.json

**Project Structure:**
- TodoApi/ - Main API project
- TodoApi.Tests/ - Comprehensive test suite
- docker-compose.yml - PostgreSQL database setup
- All backend dependencies and packages configured

### 🔄 In Progress
- None currently

### ❌ Pending Implementation

**Frontend (Next.js):**
- Authentication pages (login/signup forms)
- Task management interface (list, create, edit, delete)
- Dark/light theme toggle with localStorage
- Responsive design for desktop and mobile
- WCAG AA accessibility compliance
- API integration with backend

**Deployment:**
- Frontend deployment to Vercel
- Backend deployment to Azure/AWS
- Production database hosting
- Environment configuration for production

### 📁 Project Files Overview
- `/backend/TodoApi/` - Main API implementation
- `/backend/TodoApi.Tests/` - Test suite  
- `/docs/` - Project documentation and requirements
- `docker-compose.yml` - Database container setup
- `CLAUDE.md` - This documentation file

## Key Constraints

- MVP scope only - no email verification, mobile apps, or advanced features
- Performance target: page load under 2s
- 99.9% uptime SLA requirement
- Full responsive design for desktop and mobile browsers
- Must pass WCAG AA accessibility standards