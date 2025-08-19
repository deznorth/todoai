using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApi.Data;
using TodoApi.Services;

namespace TodoApi.Tests.Infrastructure;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public string DatabaseName { get; } = $"TestDb_{Guid.NewGuid()}";
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            // Use test-specific appsettings.json
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
        });

        builder.ConfigureServices(services =>
        {
            // Remove PostgreSQL DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<TodoContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Remove any other EF-related services to avoid conflicts
            var efDescriptors = services.Where(d => 
                d.ServiceType.Namespace?.StartsWith("Microsoft.EntityFrameworkCore") == true ||
                d.ServiceType == typeof(TodoContext)).ToList();
            
            foreach (var efDescriptor in efDescriptors)
            {
                services.Remove(efDescriptor);
            }

            // Remove existing authentication services to avoid conflicts
            var authDescriptors = services.Where(d => 
                d.ServiceType.Namespace?.StartsWith("Microsoft.AspNetCore.Authentication") == true ||
                d.ServiceType.FullName?.Contains("Authentication") == true).ToList();
            
            foreach (var authDescriptor in authDescriptors)
            {
                services.Remove(authDescriptor);
            }

            // Register in-memory database for tests
            services.AddDbContext<TodoContext>(options =>
            {
                options.UseInMemoryDatabase(DatabaseName);
                options.EnableSensitiveDataLogging();
            });

            // Re-register services that depend on DbContext
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITaskService, TaskService>();

            // Configure JWT Authentication for tests
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["auth-token"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();

            // Configure logging from appsettings.json
            services.AddLogging(builder => 
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
            });
        });

        builder.UseEnvironment("Testing");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}