using HRP.Models;
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
    public DbSet<Application>Applications { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<JobPosting> JobPostings { get; set; }
    public DbSet<JobPostingSkill>JobPostingSkills { get; set; }
    public DbSet<Offer>Offers { get; set; }
    public DbSet<Skill>Skills { get; set; }
    public DbSet<UserSkill>UserSkills { get; set; }
    
    
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
    {
    }
    
}