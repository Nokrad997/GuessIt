using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class addedHashedEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -2,
                columns: new[] { "email", "password" },
                values: new object[] { "$2a$11$QUbj1LxIEejfuoJh8TaHgeqjJcV8HoAzvsex0y8n/t7zp.bYiyk3m", "$2a$11$w8QLA0vHLr1WJ1r1aySTxOd7pkvFkttVi8aS65IKBMjepEOv6z6FC" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -1,
                columns: new[] { "email", "password" },
                values: new object[] { "$2a$11$huZlpeHenmg3Yjq8Umcu9eOCFUW4/s8VMAEk3Yvmg.YXg7lURQmVK", "$2a$11$KYB2gWb0RCyfVzR8I4rIru6.cylcjFPgoFHXq14gigVzEx7pg1ywe" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -2,
                columns: new[] { "email", "password" },
                values: new object[] { "user@user.com", "$2a$11$eF/W1DDprjanJA.ypcMjxOw1tO1rJLb4blcl9VwR1Vkifk8KJQdJG" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -1,
                columns: new[] { "email", "password" },
                values: new object[] { "admin@admin.com", "$2a$11$O25YGclykINgqKL75oJJgezwqOXEqrJ8ivzAkN2noaV7qTJ6gegH." });
        }
    }
}
