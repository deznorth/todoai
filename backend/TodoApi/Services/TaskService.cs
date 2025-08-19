using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Constants;

namespace TodoApi.Services;

public class TaskService : ITaskService
{
    private readonly TodoContext _context;

    public TaskService(TodoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskResponse>> GetTasksAsync(Guid userId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.UserId == userId && !t.IsDeleted)
            .OrderBy(t => t.CreatedAt)
            .Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                Tags = t.Tags,
                Recurrence = t.Recurrence,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return tasks;
    }

    public async Task<TaskResponse?> GetTaskByIdAsync(Guid userId, Guid taskId)
    {
        var task = await _context.Tasks
            .Where(t => t.Id == taskId && t.UserId == userId && !t.IsDeleted)
            .Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                Tags = t.Tags,
                Recurrence = t.Recurrence,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .FirstOrDefaultAsync();

        return task;
    }

    public async Task<TaskResponse> CreateTaskAsync(Guid userId, CreateTaskRequest request)
    {
        var task = new TodoTask
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Priority = request.Priority,
            Tags = request.Tags,
            Recurrence = request.Recurrence
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Priority = task.Priority,
            Tags = task.Tags,
            Recurrence = task.Recurrence,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }

    public async Task<TaskResponse> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskRequest request)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId && !t.IsDeleted);

        if (task == null)
        {
            throw new InvalidOperationException(ErrorMessages.TaskNotFoundOrAccessDenied);
        }

        if (!string.IsNullOrEmpty(request.Title))
            task.Title = request.Title;
        
        if (request.Description != null)
            task.Description = request.Description;
        
        if (request.DueDate.HasValue)
            task.DueDate = request.DueDate;
        
        if (request.Priority.HasValue)
            task.Priority = request.Priority.Value;
        
        if (request.Tags != null)
            task.Tags = request.Tags;
        
        if (request.Recurrence.HasValue)
            task.Recurrence = request.Recurrence.Value;

        task.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Priority = task.Priority,
            Tags = task.Tags,
            Recurrence = task.Recurrence,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }

    public async Task DeleteTaskAsync(Guid userId, Guid taskId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId && !t.IsDeleted);

        if (task == null)
        {
            throw new InvalidOperationException(ErrorMessages.TaskNotFoundOrAccessDenied);
        }

        task.IsDeleted = true;
        task.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}