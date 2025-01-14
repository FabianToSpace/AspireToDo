using Microsoft.EntityFrameworkCore;
using ToDoApp.Data.Models;

namespace ToDoApp.Data.Contexts;

public class ToDoContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ToDoEntry> ToDos { get; set; }
}