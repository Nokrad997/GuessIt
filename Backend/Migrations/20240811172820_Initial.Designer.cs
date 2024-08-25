﻿// <auto-generated />
using System;
using Backend.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(GuessItContext))]
    [Migration("20240811172820_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Backend.Entities.Achievement", b =>
                {
                    b.Property<int>("AchievementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("achievement_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AchievementId"));

                    b.Property<string>("AchievementDescription")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("achievement_description");

                    b.Property<string>("AchievementName")
                        .IsRequired()
                        .HasColumnType("varchar(24)")
                        .HasColumnName("achievement_name");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("AchievementId")
                        .HasName("PK_achievements");

                    b.HasIndex("AchievementName")
                        .IsUnique()
                        .HasDatabaseName("UQ_achievements_achievement_name");

                    b.ToTable("achievements", (string)null);
                });

            modelBuilder.Entity("Backend.Entities.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("city_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CityId"));

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("city_name");

                    b.Property<int>("CountryIdFk")
                        .HasColumnType("int")
                        .HasColumnName("country_id");

                    b.Property<int>("GeolocationIdFk")
                        .HasColumnType("int")
                        .HasColumnName("geolocation_id");

                    b.HasKey("CityId")
                        .HasName("PK_cities");

                    b.HasIndex("CountryIdFk");

                    b.HasIndex("GeolocationIdFk");

                    b.ToTable("cities", (string)null);
                });

            modelBuilder.Entity("Backend.Entities.Continent", b =>
                {
                    b.Property<int>("ContinentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("continent_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ContinentId"));

                    b.Property<string>("ContinentName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("continent_name");

                    b.Property<int>("GeolocationIdFk")
                        .HasColumnType("int")
                        .HasColumnName("geolocation_id");

                    b.HasKey("ContinentId")
                        .HasName("PK_continents");

                    b.HasIndex("GeolocationIdFk");

                    b.ToTable("continents", (string)null);
                });

            modelBuilder.Entity("Backend.Entities.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("country_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CountryId"));

                    b.Property<int>("ContinentIdFk")
                        .HasColumnType("int")
                        .HasColumnName("continent_id");

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("country_name");

                    b.Property<int>("GeolocationIdFk")
                        .HasColumnType("int")
                        .HasColumnName("geolocation_id");

                    b.HasKey("CountryId")
                        .HasName("PK_countries");

                    b.HasIndex("ContinentIdFk");

                    b.HasIndex("GeolocationIdFk");

                    b.ToTable("countries", (string)null);
                });

            modelBuilder.Entity("Backend.Entities.Friends", b =>
                {
                    b.Property<int>("FriendsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("friends_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FriendsId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("FriendFriendshipStatus")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("friend_status");

                    b.Property<int>("FriendIdFk")
                        .HasColumnType("int")
                        .HasColumnName("friend_id");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("UserFriendshipStatus")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_status");

                    b.Property<int>("UserIdFk")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("FriendsId")
                        .HasName("PK_friends");

                    b.HasIndex("FriendIdFk");

                    b.HasIndex("UserIdFk");

                    b.ToTable("friends", (string)null);
                });

            modelBuilder.Entity("Backend.Entities.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("game_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GameId"));

                    b.Property<decimal>("DistanceToStartingLocation")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("distance_to_starting_location");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamptz")
                        .HasColumnName("end_time");

                    b.Property<Point>("GuessedLocation")
                        .IsRequired()
                        .HasColumnType("geometry")
                        .HasColumnName("guessed_location");

                    b.Property<int>("Score")
                        .HasColumnType("int")
                        .HasColumnName("score");

                    b.Property<Point>("StartLocation")
                        .IsRequired()
                        .HasColumnType("geometry")
                        .HasColumnName("start_location");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamptz")
                        .HasColumnName("start_time");

                    b.Property<int>("UserIdFk")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("GameId")
                        .HasName("PK_games");

                    b.HasIndex("UserIdFk");

                    b.ToTable("games", (string)null);
                });

            modelBuilder.Entity("Backend.Entities.Geolocation", b =>
                {
                    b.Property<int>("GeolocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("geolocation_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GeolocationId"));

                    b.Property<Polygon>("Area")
                        .IsRequired()
                        .HasColumnType("geometry(Polygon)")
                        .HasColumnName("area");

                    b.HasKey("GeolocationId")
                        .HasName("PK_Geolocations");

                    b.ToTable("geolocations", (string)null);
                });

            modelBuilder.Entity("Backend.Entities.Leaderboard", b =>
                {
                    b.Property<int>("LeaderBoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("leaderboard_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LeaderBoardId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int")
                        .HasColumnName("total_points");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("UserIdFk")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("LeaderBoardId")
                        .HasName("PK_Leaderboard");

                    b.HasIndex("UserIdFk");

                    b.ToTable("leaderboard", (string)null);
                });

            modelBuilder.Entity("Backend.Entities.Statistics", b =>
                {
                    b.Property<int>("StatisticId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("statistic_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StatisticId"));

                    b.Property<double>("AverageScore")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("average_score");

                    b.Property<int>("HighestScore")
                        .HasColumnType("int")
                        .HasColumnName("highest_score");

                    b.Property<double>("LowestTimeInSeconds")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("lowest_time_in_seconds");

                    b.Property<int>("TotalGames")
                        .HasColumnType("int")
                        .HasColumnName("total_games");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int")
                        .HasColumnName("total_points");

                    b.Property<double>("TotalTraveledDistanceInMeters")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("total_traveled_distance_in_meters");

                    b.Property<int>("UserIdFk")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("StatisticId")
                        .HasName("PK_Statistics");

                    b.HasIndex("UserIdFk");

                    b.ToTable("statistics", (string)null);
                });

            modelBuilder.Entity("Backend.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<bool>("IsAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_admin");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(24)")
                        .HasColumnName("username");

                    b.Property<bool>("Verified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("verified");

                    b.HasKey("UserId")
                        .HasName("PK_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("UQ_users_email");

                    b.ToTable("users", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = -1,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "admin@admin.com",
                            IsAdmin = true,
                            Password = "$2a$11$m4qLXoqtrKsTkuhvpdZbEuBNG5s2M.Et0DBJi0dIZgeQjEdCvjtM6",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "admin",
                            Verified = true
                        },
                        new
                        {
                            UserId = -2,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "user@user.com",
                            IsAdmin = false,
                            Password = "$2a$11$jVqxDoEYosp6U0en8nm0yu8D4atCeQ8x5F3UV2Zpsv2Hx658BcCRe",
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "user",
                            Verified = true
                        });
                });

            modelBuilder.Entity("Backend.Entities.UserAchievements", b =>
                {
                    b.Property<int>("UserAchievementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_achievement_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserAchievementId"));

                    b.Property<int>("AchievementIdFk")
                        .HasColumnType("int")
                        .HasColumnName("achievement_id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamptz")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamptz")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("UserIdFk")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("UserAchievementId")
                        .HasName("PK_UserAchievements");

                    b.HasIndex("AchievementIdFk");

                    b.HasIndex("UserIdFk");

                    b.ToTable("user_achievements", (string)null);
                });

            modelBuilder.Entity("Backend.Entities.City", b =>
                {
                    b.HasOne("Backend.Entities.Country", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_cities_countries");

                    b.HasOne("Backend.Entities.Geolocation", "Geolocation")
                        .WithMany("Cities")
                        .HasForeignKey("GeolocationIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Cities_Geolocations");

                    b.Navigation("Country");

                    b.Navigation("Geolocation");
                });

            modelBuilder.Entity("Backend.Entities.Continent", b =>
                {
                    b.HasOne("Backend.Entities.Geolocation", "Geolocation")
                        .WithMany("Continents")
                        .HasForeignKey("GeolocationIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Continents_Geolocations");

                    b.Navigation("Geolocation");
                });

            modelBuilder.Entity("Backend.Entities.Country", b =>
                {
                    b.HasOne("Backend.Entities.Continent", "Continent")
                        .WithMany("Countries")
                        .HasForeignKey("ContinentIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_countries_continents");

                    b.HasOne("Backend.Entities.Geolocation", "Geolocation")
                        .WithMany("Countries")
                        .HasForeignKey("GeolocationIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Countries_Geolocations");

                    b.Navigation("Continent");

                    b.Navigation("Geolocation");
                });

            modelBuilder.Entity("Backend.Entities.Friends", b =>
                {
                    b.HasOne("Backend.Entities.User", "Friend")
                        .WithMany()
                        .HasForeignKey("FriendIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_friends_users_friend");

                    b.HasOne("Backend.Entities.User", "User")
                        .WithMany("Friends")
                        .HasForeignKey("UserIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_friends_users");

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Entities.Game", b =>
                {
                    b.HasOne("Backend.Entities.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Entities.Leaderboard", b =>
                {
                    b.HasOne("Backend.Entities.User", "User")
                        .WithMany("Leaderboards")
                        .HasForeignKey("UserIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_Leaderboard_Users");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Entities.Statistics", b =>
                {
                    b.HasOne("Backend.Entities.User", "User")
                        .WithMany("Statistics")
                        .HasForeignKey("UserIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Entities.UserAchievements", b =>
                {
                    b.HasOne("Backend.Entities.Achievement", "Achievement")
                        .WithMany("UserAchievements")
                        .HasForeignKey("AchievementIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_UserAchievements_Achievements");

                    b.HasOne("Backend.Entities.User", "User")
                        .WithMany("UserAchievements")
                        .HasForeignKey("UserIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_UserAchievements_Users");

                    b.Navigation("Achievement");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Entities.Achievement", b =>
                {
                    b.Navigation("UserAchievements");
                });

            modelBuilder.Entity("Backend.Entities.Continent", b =>
                {
                    b.Navigation("Countries");
                });

            modelBuilder.Entity("Backend.Entities.Country", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("Backend.Entities.Geolocation", b =>
                {
                    b.Navigation("Cities");

                    b.Navigation("Continents");

                    b.Navigation("Countries");
                });

            modelBuilder.Entity("Backend.Entities.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("Games");

                    b.Navigation("Leaderboards");

                    b.Navigation("Statistics");

                    b.Navigation("UserAchievements");
                });
#pragma warning restore 612, 618
        }
    }
}
