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
    [Migration("20240720171849_Initial")]
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
                        .HasColumnType("integer")
                        .HasColumnName("achievement_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AchievementId"));

                    b.Property<string>("AchievementDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("achievement_description");

                    b.Property<string>("AchievementName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("achievement_name");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("AchievementId");

                    b.ToTable("achievements");
                });

            modelBuilder.Entity("Backend.Entities.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("city_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CityId"));

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("city_name");

                    b.Property<int>("CountryId")
                        .HasColumnType("integer");

                    b.Property<int>("CountryIdFk")
                        .HasColumnType("integer")
                        .HasColumnName("country_id");

                    b.Property<int>("GeolocationId")
                        .HasColumnType("integer");

                    b.Property<int>("GeolocationIdFk")
                        .HasColumnType("integer")
                        .HasColumnName("geolocation_id");

                    b.HasKey("CityId");

                    b.HasIndex("CountryId");

                    b.HasIndex("GeolocationId");

                    b.ToTable("cities");
                });

            modelBuilder.Entity("Backend.Entities.Continent", b =>
                {
                    b.Property<int>("ContinentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("continent_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ContinentId"));

                    b.Property<string>("ContinentName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("continent_name");

                    b.Property<int>("GeolocationId")
                        .HasColumnType("integer");

                    b.Property<int>("GeolocationIdFk")
                        .HasColumnType("integer")
                        .HasColumnName("geolocation_id");

                    b.HasKey("ContinentId");

                    b.HasIndex("GeolocationId");

                    b.ToTable("continents");
                });

            modelBuilder.Entity("Backend.Entities.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("country_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CountryId"));

                    b.Property<int>("ContinentId")
                        .HasColumnType("integer");

                    b.Property<int>("ContinentIdFk")
                        .HasColumnType("integer")
                        .HasColumnName("continent_id");

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("country_name");

                    b.Property<int>("GeolocationId")
                        .HasColumnType("integer")
                        .HasColumnName("geolocation_id");

                    b.HasKey("CountryId");

                    b.HasIndex("ContinentId");

                    b.HasIndex("GeolocationId");

                    b.ToTable("countries");
                });

            modelBuilder.Entity("Backend.Entities.Friends", b =>
                {
                    b.Property<int>("FriendsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("friends_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FriendsId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("FriendIdFk")
                        .HasColumnType("integer")
                        .HasColumnName("friend_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("UserIdFk")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("FriendsId");

                    b.HasIndex("FriendIdFk");

                    b.HasIndex("UserId");

                    b.HasIndex("UserIdFk");

                    b.ToTable("friends");
                });

            modelBuilder.Entity("Backend.Entities.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GameId"));

                    b.Property<decimal>("DistanceToStartingLocation")
                        .HasColumnType("numeric")
                        .HasColumnName("distance_to_starting_location");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("interval")
                        .HasColumnName("end_time");

                    b.Property<decimal>("GuessedLatitude")
                        .HasColumnType("numeric")
                        .HasColumnName("guessed_latitude");

                    b.Property<decimal>("GuessedLongitude")
                        .HasColumnType("numeric")
                        .HasColumnName("guessed_longitude");

                    b.Property<int>("Score")
                        .HasColumnType("integer")
                        .HasColumnName("score");

                    b.Property<decimal>("StartLatitude")
                        .HasColumnType("numeric")
                        .HasColumnName("start_latitude");

                    b.Property<decimal>("StartLongitude")
                        .HasColumnType("numeric")
                        .HasColumnName("start_longitude");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("interval")
                        .HasColumnName("start_time");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("UserIdFk")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("games");
                });

            modelBuilder.Entity("Backend.Entities.Geolocation", b =>
                {
                    b.Property<int>("GeolocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("geolocation_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GeolocationId"));

                    b.Property<Polygon>("Area")
                        .IsRequired()
                        .HasColumnType("geometry")
                        .HasColumnName("area");

                    b.HasKey("GeolocationId");

                    b.ToTable("geolocations");
                });

            modelBuilder.Entity("Backend.Entities.Leaderboard", b =>
                {
                    b.Property<int>("LeaderBoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("leaderboard_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LeaderBoardId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("integer")
                        .HasColumnName("total_points");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("UserIdFk")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("LeaderBoardId");

                    b.HasIndex("UserId");

                    b.ToTable("leaderboard");
                });

            modelBuilder.Entity("Backend.Entities.Statistics", b =>
                {
                    b.Property<int>("StatisticId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("statistic_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StatisticId"));

                    b.Property<double>("AverageScore")
                        .HasColumnType("double precision")
                        .HasColumnName("average_score");

                    b.Property<int>("HighestScore")
                        .HasColumnType("integer")
                        .HasColumnName("highest_score");

                    b.Property<double>("LowestTimeInSeconds")
                        .HasColumnType("double precision")
                        .HasColumnName("lowest_time_in_seconds");

                    b.Property<int>("TotalGames")
                        .HasColumnType("integer")
                        .HasColumnName("total_games");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("integer")
                        .HasColumnName("total_points");

                    b.Property<double>("TotalTraveledDistanceInMeters")
                        .HasColumnType("double precision")
                        .HasColumnName("total_traveled_distance_in_meters");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("UserIdFk")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("StatisticId");

                    b.HasIndex("UserId");

                    b.ToTable("statistics");
                });

            modelBuilder.Entity("Backend.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("character varying(24)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("character varying(24)")
                        .HasColumnName("username");

                    b.Property<bool>("Verified")
                        .HasColumnType("boolean")
                        .HasColumnName("verified");

                    b.HasKey("UserId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Backend.Entities.UserAchievements", b =>
                {
                    b.Property<int>("UserAchievementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_achievement_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserAchievementId"));

                    b.Property<int>("AchievementId")
                        .HasColumnType("integer")
                        .HasColumnName("achievement_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("UserIdFk")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("UserAchievementId");

                    b.HasIndex("AchievementId");

                    b.HasIndex("UserId");

                    b.ToTable("user_achievements");
                });

            modelBuilder.Entity("Backend.Entities.City", b =>
                {
                    b.HasOne("Backend.Entities.Country", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Entities.Geolocation", "Geolocation")
                        .WithMany()
                        .HasForeignKey("GeolocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Geolocation");
                });

            modelBuilder.Entity("Backend.Entities.Continent", b =>
                {
                    b.HasOne("Backend.Entities.Geolocation", "Geolocation")
                        .WithMany()
                        .HasForeignKey("GeolocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Geolocation");
                });

            modelBuilder.Entity("Backend.Entities.Country", b =>
                {
                    b.HasOne("Backend.Entities.Continent", "Continent")
                        .WithMany("Countries")
                        .HasForeignKey("ContinentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Entities.Geolocation", "Geolocation")
                        .WithMany()
                        .HasForeignKey("GeolocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Continent");

                    b.Navigation("Geolocation");
                });

            modelBuilder.Entity("Backend.Entities.Friends", b =>
                {
                    b.HasOne("Backend.Entities.User", "Friend")
                        .WithMany()
                        .HasForeignKey("FriendIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Backend.Entities.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserId");

                    b.HasOne("Backend.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserIdFk")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Entities.Game", b =>
                {
                    b.HasOne("Backend.Entities.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Entities.Leaderboard", b =>
                {
                    b.HasOne("Backend.Entities.User", "User")
                        .WithMany("Leaderboards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Entities.Statistics", b =>
                {
                    b.HasOne("Backend.Entities.User", "User")
                        .WithMany("Statistics")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Entities.UserAchievements", b =>
                {
                    b.HasOne("Backend.Entities.Achievement", "Achievement")
                        .WithMany()
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Entities.User", "User")
                        .WithMany("UserAchievements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Achievement");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Entities.Continent", b =>
                {
                    b.Navigation("Countries");
                });

            modelBuilder.Entity("Backend.Entities.Country", b =>
                {
                    b.Navigation("Cities");
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