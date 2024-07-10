using Microsoft.EntityFrameworkCore;
using Backend.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend.Context;

    public class GuessItContext : DbContext
    {
        public GuessItContext(DbContextOptions<GuessItContext> options) 
            : base(options)
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
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
    }


