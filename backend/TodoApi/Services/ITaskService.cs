using TodoApi.DTOs;

namespace TodoApi.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskResponse>> GetTasksAsync(Guid userId);
    Task<TaskResponse?> GetTaskByIdAsync(Guid userId, Guid taskId);
    Task<TaskResponse> CreateTaskAsync(Guid userId, CreateTaskRequest request);
    Task<TaskResponse> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskRequest request);
    Task DeleteTaskAsync(Guid userId, Guid taskId);
}