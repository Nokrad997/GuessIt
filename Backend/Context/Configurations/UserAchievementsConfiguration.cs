using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Context.Configurations;

public class UserAchievementsConfiguration : IEntityTypeConfiguration<UserAchievements>
{
    public void Configure(EntityTypeBuilder<UserAchievements> builder)
    {
        //tableConfig
        builder.ToTable("user_achievements");
        builder.HasKey(ua => ua.UserAchievementId)
            .HasName("PK_UserAchievements");
        
        //properties
        builder.Property(ua => ua.UserAchievementId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasColumnName("user_achievement_id");
        builder.Property(ua => ua.UserIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("user_id");
        builder.Property(ua => ua.AchievementIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("achievement_id");
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
        builder.HasOne(ua => ua.User)
            .WithMany(u => u.UserAchievements)
            .HasForeignKey(ua => ua.UserIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_UserAchievements_Users");
        builder.HasOne(ua => ua.Achievement)
            .WithMany(a => a.UserAchievements)
            .HasForeignKey(ua => ua.AchievementIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_UserAchievements_Achievements");
    }
}