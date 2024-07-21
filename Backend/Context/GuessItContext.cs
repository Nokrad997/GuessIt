using Microsoft.EntityFrameworkCore;
using Backend.Entities;
using Backend.Entities.Interfaces;

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
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            
            modelBuilder.Entity<Friends>()
                .HasKey(f => f.FriendsId);

            modelBuilder.Entity<Friends>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserIdFk)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Friends>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendIdFk)
                .OnDelete(DeleteBehavior.Restrict);
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


