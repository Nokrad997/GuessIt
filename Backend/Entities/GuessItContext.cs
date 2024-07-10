using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Entities;

public class GuessItContext : DbContext
{
    public GuessItContext(DbContextOptions options) :base(options)
    {
    }

    public DbSet<Users> Users { get; set; }
    public DbSet<Statistics> Statistics { get; set; }
    public DbSet<Friends> Friends { get; set; }
    public DbSet<Games> Games { get; set; }
    public DbSet<Leaderboard> Leaderboard { get; set; }
    public DbSet<Achievements> Achievements { get; set; }
    public DbSet<UserAchievements> UserAchievements { get; set; }
    public DbSet<Continents> Continents { get; set; }
    public DbSet<Countries> Countries { get; set; }
    public DbSet<Cities> Cities { get; set; }
    public DbSet<Geolocations> Geolocations { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("YourConnectionString", 
            o => o.UseNetTopologySuite());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}