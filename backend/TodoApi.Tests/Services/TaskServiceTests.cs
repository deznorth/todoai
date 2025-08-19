using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using TodoApi.Data;
using TodoApi.Services;
using TodoApi.Models;
using TodoApi.DTOs;
using TodoApi.Constants;

namespace TodoApi.Tests.Services;

public class TaskServiceTests : IDisposable
{
    private readonly TodoContext _context;
    private readonly TaskService _taskService;
    private readonly Guid _testUserId = Guid.NewGuid();

    public TaskServiceTests()
    {
        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new TodoContext(options);
        _taskService = new TaskService(_context);
        
        SeedTestUser();
    }

    private void SeedTestUser()
    {
        var user = new User
        {
            Id = _testUserId,
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetTasksAsync_ShouldReturnOnlyNonDeletedTasksForUser()
    {
        var task1 = new TodoTask
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Title = "Task 1",
            Priority = Priority.Medium,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var task2 = new TodoTask
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Title = "Task 2",
            Priority = Priority.High,
            IsDeleted = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var otherUserTask = new TodoTask
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Title = "Other User Task",
            Priority = Priority.Low,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.AddRange(task1, task2, otherUserTask);
        await _context.SaveChangesAsync();

        var result = await _taskService.GetTasksAsync(_testUserId);

        result.Should().HaveCount(1);
        result.First().Id.Should().Be(task1.Id);
        result.First().Title.Should().Be("Task 1");
    }

    [Fact]
    public async Task CreateTaskAsync_WithValidData_ShouldCreateTask()
    {
        var request = new CreateTaskRequest
        {
            Title = "New Task",
            Description = "Task description",
            DueDate = DateTime.UtcNow.AddDays(7),
            Priority = Priority.High,
            Tags = new[] { "work", "urgent" },
            Recurrence = Recurrence.Weekly
        };

        var result = await _taskService.CreateTaskAsync(_testUserId, request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Title.Should().Be(request.Title);
        result.Description.Should().Be(request.Description);
        result.DueDate.Should().Be(request.DueDate);
        result.Priority.Should().Be(request.Priority);
        result.Tags.Should().BeEquivalentTo(request.Tags);
        result.Recurrence.Should().Be(request.Recurrence);

        var taskInDb = await _context.Tasks.FindAsync(result.Id);
        taskInDb.Should().NotBeNull();
        taskInDb!.UserId.Should().Be(_testUserId);
        taskInDb.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task CreateTaskAsync_WithMinimalData_ShouldCreateTask()
    {
        var request = new CreateTaskRequest
        {
            Title = "Minimal Task",
            Priority = Priority.Low
        };

        var result = await _taskService.CreateTaskAsync(_testUserId, request);

        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
        result.Description.Should().BeNull();
        result.DueDate.Should().BeNull();
        result.Tags.Should().BeNullOrEmpty();
        result.Recurrence.Should().Be(Recurrence.None);
    }

    [Fact]
    public async Task GetTaskByIdAsync_WithValidId_ShouldReturnTask()
    {
        var task = new TodoTask
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Title = "Test Task",
            Description = "Test Description",
            Priority = Priority.High,
            Tags = new[] { "test", "unit" },
            Recurrence = Recurrence.Daily,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var result = await _taskService.GetTaskByIdAsync(_testUserId, task.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(task.Id);
        result.Title.Should().Be(task.Title);
        result.Description.Should().Be(task.Description);
        result.Priority.Should().Be(task.Priority);
        result.Tags.Should().BeEquivalentTo(task.Tags);
        result.Recurrence.Should().Be(task.Recurrence);
    }

    [Fact]
    public async Task GetTaskByIdAsync_WithNonExistentId_ShouldReturnNull()
    {
        var result = await _taskService.GetTaskByIdAsync(_testUserId, Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTaskByIdAsync_WithDifferentUserId_ShouldReturnNull()
    {
        var task = new TodoTask
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Title = "Test Task",
            Priority = Priority.Medium,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var result = await _taskService.GetTaskByIdAsync(Guid.NewGuid(), task.Id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTaskByIdAsync_WithDeletedTask_ShouldReturnNull()
    {
        var task = new TodoTask
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Title = "Deleted Task",
            Priority = Priority.Low,
            IsDeleted = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var result = await _taskService.GetTaskByIdAsync(_testUserId, task.Id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateTaskAsync_WithValidData_ShouldUpdateTask()
    {
        var task = new TodoTask
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Title = "Original Title",
            Priority = Priority.Low,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var request = new UpdateTaskRequest
        {
            Title = "Updated Title",
            Description = "Updated description",
            Priority = Priority.High,
            Tags = new[] { "updated" }
        };

        var result = await _taskService.UpdateTaskAsync(_testUserId, task.Id, request);

        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
        result.Description.Should().Be(request.Description);
        result.Priority.Should().Be(request.Priority.Value);
        result.Tags.Should().BeEquivalentTo(request.Tags);
    }

    [Fact]
    public async Task UpdateTaskAsync_WithNonExistentTask_ShouldThrowException()
    {
        var request = new UpdateTaskRequest { Title = "Updated Title" };

        var act = async () => await _taskService.UpdateTaskAsync(_testUserId, Guid.NewGuid(), request);
        
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessages.TaskNotFoundOrAccessDenied);
    }

    [Fact]
    public async Task UpdateTaskAsync_WithDifferentUserId_ShouldThrowException()
    {
        var task = new TodoTask
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Title = "Test Task",
            Priority = Priority.Medium,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var request = new UpdateTaskRequest { Title = "Updated Title" };

        var act = async () => await _taskService.UpdateTaskAsync(Guid.NewGuid(), task.Id, request);
        
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessages.TaskNotFoundOrAccessDenied);
    }

    [Fact]
    public async Task DeleteTaskAsync_WithValidId_ShouldSoftDeleteTask()
    {
        var task = new TodoTask
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Title = "Task to Delete",
            Priority = Priority.Medium,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        await _taskService.DeleteTaskAsync(_testUserId, task.Id);

        var taskInDb = await _context.Tasks.FindAsync(task.Id);
        taskInDb.Should().NotBeNull();
        taskInDb!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTaskAsync_WithNonExistentId_ShouldThrowException()
    {
        var act = async () => await _taskService.DeleteTaskAsync(_testUserId, Guid.NewGuid());
        
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessages.TaskNotFoundOrAccessDenied);
    }

    [Fact]
    public async Task DeleteTaskAsync_WithDifferentUserId_ShouldThrowException()
    {
        var task = new TodoTask
        {
            Id = Guid.NewGuid(),
            UserId = _testUserId,
            Title = "Task to Delete",
            Priority = Priority.Medium,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var act = async () => await _taskService.DeleteTaskAsync(Guid.NewGuid(), task.Id);
        
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessages.TaskNotFoundOrAccessDenied);
        
        var taskInDb = await _context.Tasks.FindAsync(task.Id);
        taskInDb!.IsDeleted.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}