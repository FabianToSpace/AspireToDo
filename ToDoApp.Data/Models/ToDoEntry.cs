using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Data.Models;

[PrimaryKey(nameof(Id))]
public sealed class ToDoEntry
{
    public int Id { get; set;  }

    [MaxLength(200)] 
    [Required] 
    public string? Title { get; set; } = string.Empty;
    
    public bool? IsComplete { get; set; }
}