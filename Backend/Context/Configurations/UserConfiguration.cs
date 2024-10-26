using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Context.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //tableConfig
        builder.ToTable("users");
        builder.HasKey(u => u.UserId)
            .HasName("PK_users");
        
        //properties
        builder.Property(u => u.UserId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasColumnName("user_id");
        builder.Property(u => u.Username)
            .IsRequired()
            .HasColumnType("varchar(24)")
            .HasColumnName("username");
        builder.Property(u => u.Email)
            .IsRequired()
            .HasColumnType("varchar(255)")
            .HasColumnName("email");
        builder.Property(u => u.Password)
            .IsRequired()
            .HasColumnType("varchar(255)")
            .HasColumnName("password");
        builder.Property(u => u.Verified)
            .HasDefaultValue(false)
            .HasColumnType("boolean")
            .HasColumnName("verified");
        builder.Property(u => u.IsAdmin)
            .HasDefaultValue(false)
            .HasColumnType("boolean")
            .HasColumnName("is_admin");
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();
        
        //relations
        builder.HasMany(u => u.Statistics)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserIdFk)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(u => u.Friends)
            .WithOne(f => f.User)
            .HasForeignKey(f => f.UserIdFk)
            .OnDelete(DeleteBehavior.Restrict);
        // builder.HasMany(u => u.Friends)
        //     .WithOne(f => f.Friend)
        //     .HasForeignKey(f => f.FriendIdFk)
        //     .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(u => u.Games)
            .WithOne(g => g.User)
            .HasForeignKey(g => g.UserIdFk)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(u => u.Leaderboards)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserIdFk)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(u => u.UserAchievements)
            .WithOne(ua => ua.User)
            .HasForeignKey(ua => ua.UserIdFk)
            .OnDelete(DeleteBehavior.Restrict);
        
        //indexes
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("UQ_users_email");
    }
}