using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Context.Configurations;

public class LeaderboardConfiguration : IEntityTypeConfiguration<Leaderboard>
{
    public void Configure(EntityTypeBuilder<Leaderboard> builder)
    {
        //talbeConfig
        builder.ToTable("leaderboard");
        builder.HasKey(l => l.LeaderBoardId)
            .HasName("PK_Leaderboard");
        
        //properties
        builder.Property(l => l.LeaderBoardId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasColumnName("leaderboard_id");
        builder.Property(l => l.UserIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("user_id");
        builder.Property(l => l.TotalPoints)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("total_points");
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
        builder.HasOne(l => l.User)
            .WithMany(u => u.Leaderboards)
            .HasForeignKey(l => l.UserIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Leaderboard_Users");
    }
}