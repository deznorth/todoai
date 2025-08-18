# Landing/Login Page Prompt
Design a modern landing/login page for a To-Do web application with these specifications:

Visual Style:
- Modern, minimalist design using Tailwind CSS aesthetic
- Clean typography and proper spacing
- Accessible color contrast (WCAG AA compliant)
- Subtle shadows and rounded corners
- Professional but friendly appearance

Page Elements:
- App logo/branding at top
- Centered login form with email and password fields
- "Sign In" button (prominent, accessible)
- "Don't have an account? Sign up" link below form
- Light/dark theme toggle in header
- Form validation states (error/success indicators)
- Professional welcome message or brief app description

Layout Considerations:
- Clean, centered layout with plenty of white space
- Responsive design for desktop and mobile
- Form should be easily accessible via keyboard navigation
- Loading states for form submission
- Simple, distraction-free design that builds trust

Functionality Hints:
- Email input with proper validation styling
- Password field with show/hide toggle option
- Remember me checkbox (optional)
- Forgot password link (can be placeholder for MVP)

Focus on creating a welcoming, trustworthy first impression that encourages user signup.
# Main Dashboard Page Prompt
Design the main dashboard for a To-Do web application with these specifications:

Visual Style:
- Modern, minimalist design using Tailwind CSS aesthetic
- Clean typography and proper spacing
- Accessible color contrast (WCAG AA compliant)
- Subtle shadows and rounded corners
- Professional but friendly appearance

Header Elements:
- App logo/branding on left
- Light/dark theme toggle
- User menu/profile dropdown with logout option on right

Main Layout:
- Sidebar or top navigation for filtering: All Tasks, Today, Overdue, By Priority
- Main content area displaying task list/grid
- Prominent "Add Task" button (floating action button or header CTA)
- Search bar for finding specific tasks

Task Display:
- Task cards showing: title, description preview, due date, priority badge
- Priority indicators using color coding (red=high, yellow=medium, green=low)
- Tag system with colored pills/chips
- Task completion checkboxes on left
- Quick action buttons (edit, delete) on hover/mobile tap

Additional Features:
- Empty state design for when no tasks exist ("Ready to get organized? Add your first task!")
- Loading states for task operations
- Task count indicators in filter navigation
- Overdue tasks highlighted distinctly

Responsive Considerations:
- Mobile-first design with collapsible sidebar
- Touch-friendly task interactions
- Swipe gestures hint for mobile actions

Focus on productivity, clear task visualization, and intuitive task management workflow.

# Task Creation/Editing Modal Prompt

Design a task creation and editing modal for a To-Do web application with these specifications:

Visual Style:
- Modern, minimalist design using Tailwind CSS aesthetic
- Clean typography and proper spacing
- Accessible color contrast (WCAG AA compliant)
- Subtle shadows and rounded corners
- Professional but friendly appearance

Modal Layout:
- Centered overlay with backdrop blur/darken
- Clean white/dark (theme-aware) modal container
- Modal header with "Add Task" or "Edit Task" title
- Close button (X) in top right corner
- Proper focus trapping for accessibility

Form Elements:
- Title field: Large, prominent text input (required)
- Description field: Multi-line Text Area (optional, expandable)
- Due Date: Date picker with calendar popup
- Priority: Dropdown or radio buttons (Low/Medium/High with color indicators)
- Tags: Input field with autocomplete/suggestion chips, existing tags as removable pills
- Recurrence: Dropdown (None/Daily/Weekly/Monthly)

Action Buttons:
- Primary "Save Task" or "Update Task" button (right, prominent)
- Secondary "Cancel" button (left)
- For editing: "Delete Task" button (left, danger styling)

Interaction States:
- Form validation with inline error messages
- Loading state during save operations
- Success/error feedback
- Autosave indicator (optional)

Responsive Design:
- Full-screen on mobile devices
- Proper keyboard navigation support
- Touch-friendly form controls
- Scrollable content if needed

Accessibility Features:
- Proper ARIA labels and roles
- Screen reader announcements
- Keyboard shortcuts (Escape to close, Enter to save)
- Focus management when opening/closing

Focus on creating an efficient, user-friendly task entry experience that minimizes friction while capturing all necessary task details.