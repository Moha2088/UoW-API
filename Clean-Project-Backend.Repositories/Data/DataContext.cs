
using Clean_Project_Backend.Repositories.Entities;
using Clean_Project_Backend.Repositories.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Clean_Project_Backend.Repositories.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }   

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Applies all the entitytypeconfigurations from the same assembly as the UserEntity
        builder.ApplyConfigurationsFromAssembly(typeof(UserEntitytypeConfiguration).Assembly);
    }

    public DbSet<Project> Projects { get; set; }

    public DbSet<User> Users { get; set; }
}
