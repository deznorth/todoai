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
- POST /auth/signup - Register new user
- POST /auth/login - Authenticate and return JWT

**Tasks:**
- GET /tasks - Get all non-deleted tasks for authenticated user
- POST /tasks - Create new task
- PUT /tasks/{task_id} - Update existing task (owner only)
- DELETE /tasks/{task_id} - Soft delete task (owner only)

## Security Requirements

- Passwords hashed with bcrypt (work factor 12)
- JWT tokens stored in secure HTTP-only cookies (24-hour expiry)
- User authorization: users can only access/modify their own tasks
- Input validation on all endpoints
- OWASP best practices compliance

## Development Status

This is a greenfield project with comprehensive PRD and user stories defined but no code implementation yet. All features need to be built from scratch following the specifications in the documents/ folder.

## Key Constraints

- MVP scope only - no email verification, mobile apps, or advanced features
- Performance target: page load under 2s
- 99.9% uptime SLA requirement
- Full responsive design for desktop and mobile browsers
- Must pass WCAG AA accessibility standards