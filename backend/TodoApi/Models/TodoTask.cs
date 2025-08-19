using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models;

public enum Priority
{
    Low,
    Medium,
    High
}

public enum Recurrence
{
    None,
    Daily,
    Weekly,
    Monthly
}

public class TodoTask
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    [Required]
    public Priority Priority { get; set; } = Priority.Medium;
    
    public string[]? Tags { get; set; }
    
    public Recurrence Recurrence { get; set; } = Recurrence.None;
    
    public bool IsDeleted { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}