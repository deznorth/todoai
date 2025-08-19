using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using TodoApi.DTOs;
using TodoApi.Tests.Infrastructure;

namespace TodoApi.Tests.Controllers;

public class AuthControllerTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public AuthControllerTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ShouldReturnCreated()
    {
        var request = new SignupRequest
        {
            Email = "test@example.com",
            Password = "SecurePass123!"
        };

        var response = await _client.PostAsJsonAsync("/auth/signup", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var content = await response.Content.ReadFromJsonAsync<AuthResponse>();
        content.Should().NotBeNull();
        content!.Email.Should().Be(request.Email);
        content.UserId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ShouldReturnBadRequest()
    {
        var request = new SignupRequest
        {
            Email = "duplicate@example.com",
            Password = "SecurePass123!"
        };

        await _client.PostAsJsonAsync("/auth/signup", request);
        var response = await _client.PostAsJsonAsync("/auth/signup", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("", "SecurePass123!")]
    [InlineData("invalid-email", "SecurePass123!")]
    [InlineData("test@example.com", "")]
    [InlineData("test@example.com", "short")]
    public async Task Register_WithInvalidData_ShouldReturnBadRequest(string email, string password)
    {
        var request = new SignupRequest
        {
            Email = email,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("/auth/signup", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnOkWithCookie()
    {
        var registerRequest = new SignupRequest
        {
            Email = "login@example.com",
            Password = "SecurePass123!"
        };
        await _client.PostAsJsonAsync("/auth/signup", registerRequest);

        var loginRequest = new LoginRequest
        {
            Email = "login@example.com",
            Password = "SecurePass123!"
        };

        var response = await _client.PostAsJsonAsync("/auth/login", loginRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadFromJsonAsync<AuthResponse>();
        content.Should().NotBeNull();
        content!.Email.Should().Be(loginRequest.Email);

        response.Headers.Should().Contain(h => h.Key == "Set-Cookie");
        var setCookieHeader = response.Headers.GetValues("Set-Cookie").First();
        setCookieHeader.Should().Contain("auth-token=");
        setCookieHeader.Should().Contain("httponly", "cookie should be HTTP-only");
        setCookieHeader.Should().Contain("secure", "cookie should be secure");
        setCookieHeader.Should().Contain("samesite=strict", "cookie should have strict same-site policy");
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        var request = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "wrongpassword"
        };

        var response = await _client.PostAsJsonAsync("/auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("", "SecurePass123!")]
    [InlineData("invalid-email", "SecurePass123!")]
    [InlineData("test@example.com", "")]
    public async Task Login_WithInvalidData_ShouldReturnBadRequest(string email, string password)
    {
        var request = new LoginRequest
        {
            Email = email,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("/auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_WithCorrectEmailButWrongPassword_ShouldReturnUnauthorized()
    {
        var registerRequest = new SignupRequest
        {
            Email = "correctemail@example.com",
            Password = "CorrectPassword123!"
        };
        await _client.PostAsJsonAsync("/auth/signup", registerRequest);

        var loginRequest = new LoginRequest
        {
            Email = "correctemail@example.com",
            Password = "WrongPassword123!"
        };

        var response = await _client.PostAsJsonAsync("/auth/login", loginRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task<string> RegisterAndLoginAsync(string email = "auth@example.com", string password = "SecurePass123!")
    {
        var registerRequest = new SignupRequest { Email = email, Password = password };
        await _client.PostAsJsonAsync("/auth/signup", registerRequest);

        var loginRequest = new LoginRequest { Email = email, Password = password };
        var loginResponse = await _client.PostAsJsonAsync("/auth/login", loginRequest);

        var setCookieHeader = loginResponse.Headers.GetValues("Set-Cookie").First();
        var token = setCookieHeader.Split(';')[0].Split('=')[1];
        return token;
    }
}