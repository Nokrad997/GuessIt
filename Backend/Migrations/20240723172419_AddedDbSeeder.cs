using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedDbSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "email", "is_admin", "password", "username", "verified" },
                values: new object[] { -1, "admin@admin.com", true, "$2a$11$tbZ.6GZkalk5Ctx9fMQSwuD25mo6UWDJAS8LHXdvCZcLc9fQE0zm6", "admin", true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -1);
        }
    }
}
