using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangedGeolocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -2,
                column: "password",
                value: "$2a$11$2DI4diiO/ROyvzGiwD1a8.vmp6vwyDtiWXqX5SgDCh9TzJ2QCwSye");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -1,
                column: "password",
                value: "$2a$11$wY.Siub1Hg5ZZMP0trwtR.FSsPL4r1x7VM6IUqw4xtA8bQ/61PHpy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -2,
                column: "password",
                value: "$2a$11$jVqxDoEYosp6U0en8nm0yu8D4atCeQ8x5F3UV2Zpsv2Hx658BcCRe");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -1,
                column: "password",
                value: "$2a$11$m4qLXoqtrKsTkuhvpdZbEuBNG5s2M.Et0DBJi0dIZgeQjEdCvjtM6");
        }
    }
}
