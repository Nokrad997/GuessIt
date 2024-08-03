// Backend/Context/Configurations/FriendsConfiguration.cs
using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Context.Configurations;

public class FriendsConfiguration : IEntityTypeConfiguration<Friends>
{
    public void Configure(EntityTypeBuilder<Friends> builder)
    {
        //tableConfig
        builder.ToTable("friends");
        builder.HasKey(f => f.FriendsId)
            .HasName("PK_friends");

        //properties
        builder.Property(f => f.FriendsId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnType("int")
            .HasColumnName("friends_id");
        builder.Property(f => f.UserIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("user_id");
        builder.Property(f => f.FriendIdFk)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("friend_id");
        builder.Property(f => f.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnName("status");
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
        builder.HasOne(f => f.User)
            .WithMany(u => u.Friends)
            .HasForeignKey(f => f.UserIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_friends_users");

        builder.HasOne(f => f.Friend)
            .WithMany()
            .HasForeignKey(f => f.FriendIdFk)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_friends_users_friend");
    }
}