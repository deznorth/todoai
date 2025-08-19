using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Tests.Infrastructure;

namespace TodoApi.Tests.Controllers;

public class TasksControllerTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public TasksControllerTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetTasks_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        var response = await _client.GetAsync("/tasks");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetTasks_WithAuthentication_ShouldReturnTasks()
    {
        var token = await RegisterAndLoginAsync();
        await CreateSampleTaskAsync(token);

        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");
        var response = await _client.GetAsync("/tasks");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskResponse>>();
        tasks.Should().NotBeNull();
        tasks!.Should().HaveCount(1);
        tasks.First().Title.Should().Be("Sample Task");
    }

    [Fact]
    public async Task GetTaskById_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        var response = await _client.GetAsync($"/tasks/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetTaskById_WithValidId_ShouldReturnTask()
    {
        var token = await RegisterAndLoginAsync();
        var taskId = await CreateSampleTaskAsync(token);

        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");
        var response = await _client.GetAsync($"/tasks/{taskId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var task = await response.Content.ReadFromJsonAsync<TaskResponse>();
        task.Should().NotBeNull();
        task!.Id.Should().Be(taskId);
        task.Title.Should().Be("Sample Task");
        task.Priority.Should().Be(Priority.Medium);
    }

    [Fact]
    public async Task GetTaskById_WithNonExistentId_ShouldReturnNotFound()
    {
        var token = await RegisterAndLoginAsync();
        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");

        var response = await _client.GetAsync($"/tasks/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTaskById_WithDifferentUserTask_ShouldReturnNotFound()
    {
        var token1 = await RegisterAndLoginAsync("user1@example.com");
        var taskId = await CreateSampleTaskAsync(token1);

        var token2 = await RegisterAndLoginAsync("user2@example.com");
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token2}");

        var response = await _client.GetAsync($"/tasks/{taskId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTaskById_WithInvalidGuid_ShouldReturnBadRequest()
    {
        var token = await RegisterAndLoginAsync();
        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");

        var response = await _client.GetAsync("/tasks/invalid-guid");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTask_WithValidData_ShouldReturnCreated()
    {
        var token = await RegisterAndLoginAsync();
        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");

        var request = new CreateTaskRequest
        {
            Title = "New Task",
            Description = "Task description",
            DueDate = DateTime.UtcNow.AddDays(7),
            Priority = Priority.High,
            Tags = new[] { "work", "urgent" },
            Recurrence = Recurrence.Weekly
        };

        var response = await _client.PostAsJsonAsync("/tasks", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var task = await response.Content.ReadFromJsonAsync<TaskResponse>();
        task.Should().NotBeNull();
        task!.Title.Should().Be(request.Title);
        task.Description.Should().Be(request.Description);
        task.Priority.Should().Be(request.Priority);
        task.Tags.Should().BeEquivalentTo(request.Tags);
        task.Recurrence.Should().Be(request.Recurrence);
    }

    [Fact]
    public async Task CreateTask_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        var request = new CreateTaskRequest
        {
            Title = "New Task",
            Priority = Priority.Medium
        };

        var response = await _client.PostAsJsonAsync("/tasks", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("", Priority.Medium)]
    [InlineData(null, Priority.Medium)]
    public async Task CreateTask_WithInvalidData_ShouldReturnBadRequest(string? title, Priority priority)
    {
        var token = await RegisterAndLoginAsync();
        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");

        var request = new CreateTaskRequest
        {
            Title = title!,
            Priority = priority
        };

        var response = await _client.PostAsJsonAsync("/tasks", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTask_WithValidData_ShouldReturnOk()
    {
        var token = await RegisterAndLoginAsync();
        var taskId = await CreateSampleTaskAsync(token);

        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");

        var request = new UpdateTaskRequest
        {
            Title = "Updated Task",
            Description = "Updated description",
            Priority = Priority.Low,
            Tags = new[] { "updated" }
        };

        var response = await _client.PutAsJsonAsync($"/tasks/{taskId}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var task = await response.Content.ReadFromJsonAsync<TaskResponse>();
        task.Should().NotBeNull();
        task!.Title.Should().Be(request.Title);
        task.Description.Should().Be(request.Description);
        task.Priority.Should().Be(request.Priority.Value);
        task.Tags.Should().BeEquivalentTo(request.Tags);
    }

    [Fact]
    public async Task UpdateTask_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        var request = new UpdateTaskRequest
        {
            Title = "Updated Task"
        };

        var response = await _client.PutAsJsonAsync($"/tasks/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateTask_WithNonExistentTask_ShouldReturnNotFound()
    {
        var token = await RegisterAndLoginAsync();
        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");

        var request = new UpdateTaskRequest
        {
            Title = "Updated Task"
        };

        var response = await _client.PutAsJsonAsync($"/tasks/{Guid.NewGuid()}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTask_WithDifferentUserTask_ShouldReturnNotFound()
    {
        var token1 = await RegisterAndLoginAsync("user1@example.com");
        var taskId = await CreateSampleTaskAsync(token1);

        var token2 = await RegisterAndLoginAsync("user2@example.com");
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token2}");

        var request = new UpdateTaskRequest
        {
            Title = "Updated Task"
        };

        var response = await _client.PutAsJsonAsync($"/tasks/{taskId}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTask_WithValidId_ShouldReturnNoContent()
    {
        var token = await RegisterAndLoginAsync();
        var taskId = await CreateSampleTaskAsync(token);

        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");

        var response = await _client.DeleteAsync($"/tasks/{taskId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the specific task was deleted by trying to get it by ID
        var getResponse = await _client.GetAsync($"/tasks/{taskId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTask_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        var response = await _client.DeleteAsync($"/tasks/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteTask_WithNonExistentTask_ShouldReturnNotFound()
    {
        var token = await RegisterAndLoginAsync();
        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");

        var response = await _client.DeleteAsync($"/tasks/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTask_WithDifferentUserTask_ShouldReturnNotFound()
    {
        var token1 = await RegisterAndLoginAsync("user1@example.com");
        var taskId = await CreateSampleTaskAsync(token1);

        var token2 = await RegisterAndLoginAsync("user2@example.com");
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token2}");

        var response = await _client.DeleteAsync($"/tasks/{taskId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task TasksWorkflow_CreateUpdateDelete_ShouldWorkCorrectly()
    {
        var token = await RegisterAndLoginAsync();
        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");

        var createRequest = new CreateTaskRequest
        {
            Title = "Workflow Task",
            Priority = Priority.Medium
        };
        var createResponse = await _client.PostAsJsonAsync("/tasks", createRequest);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>();

        var updateRequest = new UpdateTaskRequest
        {
            Title = "Updated Workflow Task",
            Priority = Priority.High
        };
        var updateResponse = await _client.PutAsJsonAsync($"/tasks/{createdTask!.Id}", updateRequest);
        var updatedTask = await updateResponse.Content.ReadFromJsonAsync<TaskResponse>();

        updatedTask!.Title.Should().Be("Updated Workflow Task");
        updatedTask.Priority.Should().Be(Priority.High);

        var deleteResponse = await _client.DeleteAsync($"/tasks/{createdTask.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the specific task was deleted by trying to get it by ID
        var getResponse = await _client.GetAsync($"/tasks/{createdTask.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<string> RegisterAndLoginAsync(string email = "test@example.com", string password = "SecurePass123!")
    {
        var registerRequest = new SignupRequest { Email = email, Password = password };
        await _client.PostAsJsonAsync("/auth/signup", registerRequest);

        var loginRequest = new LoginRequest { Email = email, Password = password };
        var loginResponse = await _client.PostAsJsonAsync("/auth/login", loginRequest);

        var setCookieHeader = loginResponse.Headers.GetValues("Set-Cookie").First();
        var token = setCookieHeader.Split(';')[0].Split('=')[1];
        return token;
    }

    private async Task<Guid> CreateSampleTaskAsync(string token)
    {
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Cookie", $"auth-token={token}");

        var request = new CreateTaskRequest
        {
            Title = "Sample Task",
            Priority = Priority.Medium
        };

        var response = await _client.PostAsJsonAsync("/tasks", request);
        var task = await response.Content.ReadFromJsonAsync<TaskResponse>();
        return task!.Id;
    }
}