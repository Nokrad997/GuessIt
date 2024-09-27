using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class changedAreaInGeolocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Geometry>(
                name: "area",
                table: "geolocations",
                type: "geometry",
                nullable: false,
                oldClrType: typeof(Polygon),
                oldType: "geometry(Polygon)");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -2,
                column: "password",
                value: "$2a$11$ATJ75mojmAEanWV6kmfWPuuJtCj4Q7FgBljz0cEntZd4Jnq0NYn6q");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -1,
                column: "password",
                value: "$2a$11$Utwx/llJdHkdubeEu/38QeEOKsqGshl4khewcTW.lMmvscEWux95u");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Polygon>(
                name: "area",
                table: "geolocations",
                type: "geometry(Polygon)",
                nullable: false,
                oldClrType: typeof(Geometry),
                oldType: "geometry");

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
