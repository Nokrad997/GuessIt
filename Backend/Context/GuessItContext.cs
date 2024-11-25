using Backend.Context.Configurations;
using Backend.Controllers;
using Microsoft.EntityFrameworkCore;
using Backend.Entities;
using Backend.Entities.Interfaces;
using Backend.Utility;

namespace Backend.Context;

    public class GuessItContext : DbContext
    {
        public GuessItContext(DbContextOptions<GuessItContext> options) 
            : base(options)
        {
        }
        
        public DbSet<User> User { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<Friends> Friends { get; set; }
        public DbSet<Game> Game { get; set; }
        public DbSet<Leaderboard> Leaderboard { get; set; }
        public DbSet<Achievement> Achievement { get; set; }
        public DbSet<UserAchievements> UserAchievements { get; set; }
        public DbSet<Continent> Continent { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Geolocation> Geolocation { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new DbInitializer(modelBuilder, new PasswordAndEmailHasher()).seed();

            modelBuilder.ApplyConfiguration(new FriendsConfiguration());
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new LeaderboardConfiguration());
            modelBuilder.ApplyConfiguration(new AchievementsConfiguration());
            modelBuilder.ApplyConfiguration(new UserAchievementsConfiguration());
            modelBuilder.ApplyConfiguration(new ContinentConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new GeolocationConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new StatisticsConfiguration());
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
        
        public override int SaveChanges()
        {
            SetTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetTimestamps()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is IHasTimeStamp);
            var now = DateTime.UtcNow;
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    
                    ((IHasTimeStamp)entry.Entity).CreatedAt = now;
                    ((IHasTimeStamp)entry.Entity).UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    ((IHasTimeStamp)entry.Entity).UpdatedAt = now;
                }
            }
        }
    }


