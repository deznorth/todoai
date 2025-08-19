namespace TodoApi.Constants;

public static class ErrorMessages
{
    // Authentication related errors
    public const string UserWithEmailAlreadyExists = "User with this email already exists";
    public const string InvalidCredentials = "Invalid email or password";
    
    // Task related errors
    public const string TaskNotFoundOrAccessDenied = "Task not found or access denied";
    
    // General errors
    public const string SignupError = "An error occurred during signup";
    public const string LoginError = "An error occurred during login";
    
    // Success messages
    public const string UserCreatedSuccessfully = "User created successfully";
    public const string LoginSuccessful = "Login successful";
    public const string LogoutSuccessful = "Logout successful";
}