using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addedTraveledDistanceToGameEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "traveled_distance",
                table: "games",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -2,
                column: "password",
                value: "$2a$11$eF/W1DDprjanJA.ypcMjxOw1tO1rJLb4blcl9VwR1Vkifk8KJQdJG");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -1,
                column: "password",
                value: "$2a$11$O25YGclykINgqKL75oJJgezwqOXEqrJ8ivzAkN2noaV7qTJ6gegH.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "traveled_distance",
                table: "games");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -2,
                column: "password",
                value: "$2a$11$uZKRrfsi6WpztMZLtKv/9ORPm.TwqeTC8SIuaK.8f0UIL3WgmQu9W");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -1,
                column: "password",
                value: "$2a$11$XLzxxqQMi./5XMubIm9mN.Dim5tqf2FSfVY8OcLma1FuEmUlFJQ1K");
        }
    }
}
