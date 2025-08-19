using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using TodoApi.Data;
using TodoApi.Services;
using TodoApi.Models;
using TodoApi.DTOs;
using TodoApi.Constants;

namespace TodoApi.Tests.Services;

public class AuthServiceTests : IDisposable
{
    private readonly TodoContext _context;
    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new TodoContext(options);
        
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            {"JwtSettings:SecretKey", "ThisIsATestSecretKeyThatIsLongEnoughForHS256"},
            {"JwtSettings:Issuer", "TestIssuer"},
            {"JwtSettings:Audience", "TestAudience"},
            {"JwtSettings:ExpirationHours", "24"}
        });
        _configuration = configBuilder.Build();
        
        _authService = new AuthService(_context, _configuration);
    }

    [Fact]
    public async Task SignupAsync_WithValidData_ShouldCreateUser()
    {
        var request = new SignupRequest
        {
            Email = "test@example.com",
            Password = "SecurePass123!"
        };

        var result = await _authService.SignupAsync(request);

        result.Should().NotBeNull();
        result.Email.Should().Be(request.Email);
        result.UserId.Should().NotBeEmpty();

        var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        userInDb.Should().NotBeNull();
        userInDb!.Email.Should().Be(request.Email);
        BCrypt.Net.BCrypt.Verify(request.Password, userInDb.PasswordHash).Should().BeTrue();
    }

    [Fact]
    public async Task SignupAsync_WithDuplicateEmail_ShouldThrowException()
    {
        var email = "test@example.com";
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();

        var request = new SignupRequest
        {
            Email = email,
            Password = "SecurePass123!"
        };

        var act = async () => await _authService.SignupAsync(request);
        
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessages.UserWithEmailAlreadyExists);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnUser()
    {
        var email = "test@example.com";
        var password = "SecurePass123!";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var request = new LoginRequest
        {
            Email = email,
            Password = password
        };

        var result = await _authService.LoginAsync(request);

        result.Should().NotBeNull();
        result.Email.Should().Be(email);
        result.UserId.Should().Be(user.Id);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidEmail_ShouldThrowException()
    {
        var request = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "password"
        };

        var act = async () => await _authService.LoginAsync(request);
        
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage(ErrorMessages.InvalidCredentials);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldThrowException()
    {
        var email = "test@example.com";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword"),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var request = new LoginRequest
        {
            Email = email,
            Password = "wrongpassword"
        };

        var act = async () => await _authService.LoginAsync(request);
        
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage(ErrorMessages.InvalidCredentials);
    }

    [Fact]
    public void GenerateJwtToken_WithValidData_ShouldReturnToken()
    {
        var userId = Guid.NewGuid();
        var email = "test@example.com";

        var token = _authService.GenerateJwtToken(userId, email);

        token.Should().NotBeNullOrEmpty();
        token.Split('.').Should().HaveCount(3); // JWT has 3 parts
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}