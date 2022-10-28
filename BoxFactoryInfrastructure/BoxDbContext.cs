using BoxFactoryApp;
using Microsoft.EntityFrameworkCore;

namespace BoxFactoryInfrastructure;

public class BoxDbContext : DbContext
{
    public BoxDbContext(DbContextOptions<BoxDbContext> opts) : base(opts)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Box>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
    } 
    
    public DbSet<Box> BoxTable { get; set; }
}