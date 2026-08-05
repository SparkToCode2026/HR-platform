using Microsoft.EntityFrameworkCore;
using ProjectX.Models;

namespace ProjectX;

public class ProjectContext : DbContext
{
    public DbSet<User> users { get; set; }
    public DbSet<company> Companies { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<JopCategory> JopCategories { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<Review> Reviews { get; set; }
    
    
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
    {
    }
    
}