using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Context.Configurations;

public class StatisticsConfiguration : IEntityTypeConfiguration<Statistics>
{
    public void Configure(EntityTypeBuilder<Statistics> builder)
    {
        //tableConfig
        builder.ToTable("statistics");
        builder.HasKey(s => s.StatisticId)
            .HasName("PK_Statistics");
        
        //properties
        builder.Property(s => s.StatisticId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasColumnName("statistic_id");
        builder.Property(s => s.UserIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("user_id");
        builder.Property(s => s.TotalGames)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("total_games");
        builder.Property(s => s.TotalPoints)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("total_points");
        builder.Property(s => s.HighestScore)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("highest_score");
        builder.Property(s => s.LowestTimeInSeconds)
            .IsRequired()
            .HasColumnType("decimal(10, 2)")
            .HasColumnName("lowest_time_in_seconds");
        builder.Property(s => s.TotalTraveledDistanceInMeters)
            .IsRequired()
            .HasColumnType("decimal(10, 2)")
            .HasColumnName("total_traveled_distance_in_meters");
        builder.Property(s => s.AverageScore)
            .IsRequired()
            .HasColumnType("decimal(10, 2)")
            .HasColumnName("average_score");
    }
}