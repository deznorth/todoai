using System.ComponentModel.DataAnnotations;
using TodoApi.Models;

namespace TodoApi.DTOs;

public class CreateTaskRequest
{
    [Required]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    [Required]
    public Priority Priority { get; set; } = Priority.Medium;
    
    public string[]? Tags { get; set; }
    
    public Recurrence Recurrence { get; set; } = Recurrence.None;
}

public class UpdateTaskRequest
{
    [StringLength(255)]
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public Priority? Priority { get; set; }
    
    public string[]? Tags { get; set; }
    
    public Recurrence? Recurrence { get; set; }
}

public class TaskResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Priority Priority { get; set; }
    public string[]? Tags { get; set; }
    public Recurrence Recurrence { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}