using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Controllers;

public class AchievementsConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        //tableConfig
        builder.ToTable("achievements");
        builder.HasKey(a => a.AchievementId)
            .HasName("PK_achievements");
        
        //properties
        builder.Property(a => a.AchievementId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasColumnName("achievement_id");
        builder.Property(a => a.AchievementName)
            .IsRequired()
            .HasColumnType("varchar(24)")
            .HasColumnName("achievement_name");
        builder.Property(a => a.AchievementDescription)
            .IsRequired()
            .HasColumnType("varchar(255)")
            .HasColumnName("achievement_description");
        builder.Property(a => a.AchievementCriteria)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasColumnName("achievement_criteria");
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
        
        //indexes
        builder.HasIndex(a => a.AchievementName)
            .IsUnique()
            .HasDatabaseName("UQ_achievements_achievement_name");
    }
}