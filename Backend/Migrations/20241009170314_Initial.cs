using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "achievements",
                columns: table => new
                {
                    achievement_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    achievement_name = table.Column<string>(type: "varchar(24)", nullable: false),
                    achievement_description = table.Column<string>(type: "varchar(255)", nullable: false),
                    achievement_criteria = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_achievements", x => x.achievement_id);
                });

            migrationBuilder.CreateTable(
                name: "geolocations",
                columns: table => new
                {
                    geolocation_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    area = table.Column<Geometry>(type: "geometry", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geolocations", x => x.geolocation_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "varchar(24)", nullable: false),
                    email = table.Column<string>(type: "varchar(50)", nullable: false),
                    password = table.Column<string>(type: "varchar(255)", nullable: false),
                    verified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_admin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "continents",
                columns: table => new
                {
                    continent_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    continent_name = table.Column<string>(type: "varchar(255)", nullable: false),
                    geolocation_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_continents", x => x.continent_id);
                    table.ForeignKey(
                        name: "FK_Continents_Geolocations",
                        column: x => x.geolocation_id,
                        principalTable: "geolocations",
                        principalColumn: "geolocation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "friends",
                columns: table => new
                {
                    friends_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    friend_id = table.Column<int>(type: "int", nullable: false),
                    user_status = table.Column<string>(type: "text", nullable: false),
                    friend_status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_friends", x => x.friends_id);
                    table.ForeignKey(
                        name: "FK_friends_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_friends_users_friend",
                        column: x => x.friend_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    game_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    start_location = table.Column<Point>(type: "geometry", nullable: false),
                    guessed_location = table.Column<Point>(type: "geometry", nullable: false),
                    distance_to_starting_location = table.Column<double>(type: "numeric(10,2)", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.game_id);
                    table.ForeignKey(
                        name: "FK_games_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "leaderboard",
                columns: table => new
                {
                    leaderboard_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    total_points = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaderboard", x => x.leaderboard_id);
                    table.ForeignKey(
                        name: "FK_Leaderboard_Users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "statistics",
                columns: table => new
                {
                    statistic_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    total_games = table.Column<int>(type: "int", nullable: false),
                    total_points = table.Column<int>(type: "int", nullable: false),
                    highest_score = table.Column<int>(type: "int", nullable: false),
                    lowest_time_in_seconds = table.Column<double>(type: "numeric(10,2)", nullable: false),
                    total_traveled_distance_in_meters = table.Column<double>(type: "numeric(10,2)", nullable: false),
                    average_score = table.Column<double>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.statistic_id);
                    table.ForeignKey(
                        name: "FK_statistics_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_achievements",
                columns: table => new
                {
                    user_achievement_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    achievement_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAchievements", x => x.user_achievement_id);
                    table.ForeignKey(
                        name: "FK_UserAchievements_Achievements",
                        column: x => x.achievement_id,
                        principalTable: "achievements",
                        principalColumn: "achievement_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAchievements_Users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    country_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_name = table.Column<string>(type: "varchar(255)", nullable: false),
                    continent_id = table.Column<int>(type: "int", nullable: false),
                    geolocation_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.country_id);
                    table.ForeignKey(
                        name: "FK_Countries_Geolocations",
                        column: x => x.geolocation_id,
                        principalTable: "geolocations",
                        principalColumn: "geolocation_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_countries_continents",
                        column: x => x.continent_id,
                        principalTable: "continents",
                        principalColumn: "continent_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    city_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city_name = table.Column<string>(type: "varchar(255)", nullable: false),
                    country_id = table.Column<int>(type: "int", nullable: false),
                    geolocation_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.city_id);
                    table.ForeignKey(
                        name: "FK_Cities_Geolocations",
                        column: x => x.geolocation_id,
                        principalTable: "geolocations",
                        principalColumn: "geolocation_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cities_countries",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "email", "password", "username", "verified" },
                values: new object[] { -2, "user@user.com", "$2a$11$AbEHplHmeYTIoWzsM2FTz.3UwAz/gl8X6QCOu0t2m4g/fkwN4gbXS", "user", true });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "email", "is_admin", "password", "username", "verified" },
                values: new object[] { -1, "admin@admin.com", true, "$2a$11$AcRTAU40ViqK/8EMKYyCIurHwOsQ.TaSf3atWL7Zg4uUwK6YCcesy", "admin", true });

            migrationBuilder.CreateIndex(
                name: "UQ_achievements_achievement_name",
                table: "achievements",
                column: "achievement_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cities_country_id",
                table: "cities",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_cities_geolocation_id",
                table: "cities",
                column: "geolocation_id");

            migrationBuilder.CreateIndex(
                name: "IX_continents_geolocation_id",
                table: "continents",
                column: "geolocation_id");

            migrationBuilder.CreateIndex(
                name: "IX_countries_continent_id",
                table: "countries",
                column: "continent_id");

            migrationBuilder.CreateIndex(
                name: "IX_countries_geolocation_id",
                table: "countries",
                column: "geolocation_id");

            migrationBuilder.CreateIndex(
                name: "IX_friends_friend_id",
                table: "friends",
                column: "friend_id");

            migrationBuilder.CreateIndex(
                name: "IX_friends_user_id",
                table: "friends",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_games_user_id",
                table: "games",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_leaderboard_user_id",
                table: "leaderboard",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_statistics_user_id",
                table: "statistics",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_achievements_achievement_id",
                table: "user_achievements",
                column: "achievement_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_achievements_user_id",
                table: "user_achievements",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "friends");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "leaderboard");

            migrationBuilder.DropTable(
                name: "statistics");

            migrationBuilder.DropTable(
                name: "user_achievements");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "achievements");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "continents");

            migrationBuilder.DropTable(
                name: "geolocations");
        }
    }
}
