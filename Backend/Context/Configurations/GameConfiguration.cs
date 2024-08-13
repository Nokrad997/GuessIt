using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Context.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        //tableConfig
        builder.ToTable("games");
        builder.HasKey(g => g.GameId)
            .HasName("PK_games");

        //properties
        builder.Property(g => g.GameId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasColumnName("game_id");
        builder.Property(g => g.UserIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("user_id");
        builder.Property(g => g.StartLocation)
            .IsRequired()
            .HasColumnType("geometry")
            .HasColumnName("start_location");
        builder.Property(g => g.GuessedLocation)
            .IsRequired()
            .HasColumnType("geometry")
            .HasColumnName("guessed_location");
        builder.Property(g => g.DistanceToStartingLocation)
            .IsRequired()
            .HasColumnType("decimal(10, 2)")
            .HasColumnName("distance_to_starting_location");
        builder.Property(g => g.StartTime)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasColumnName("start_time");
        builder.Property(g => g.EndTime)
            .IsRequired()
            .HasColumnType("timestamptz")
            .HasColumnName("end_time");
        builder.Property(g => g.Score)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("score");

        builder.HasOne(g => g.User)
            .WithMany(u => u.Games)
            .HasForeignKey(g => g.UserIdFk)
            .OnDelete(DeleteBehavior.Restrict);
    }
}