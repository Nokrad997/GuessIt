using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class changedGeoloctionsv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<MultiPolygon>(
                name: "area",
                table: "geolocations",
                type: "geometry(MultiPolygon)",
                nullable: false,
                oldClrType: typeof(MultiPolygon),
                oldType: "geometry(Polygon)");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -2,
                column: "password",
                value: "$2a$11$Erjca9S3hekv/UBUo2WcvuX2eB2kAYoLGJslEiuBzRQkLGWs0x9Ri");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: -1,
                column: "password",
                value: "$2a$11$tgAsCs.Gnwn4S8X30xSQn.g5P6VfQ1Mw7e8YwrbBdhLgFeqTmXRSq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<MultiPolygon>(
                name: "area",
                table: "geolocations",
                type: "geometry(Polygon)",
                nullable: false,
                oldClrType: typeof(MultiPolygon),
                oldType: "geometry(MultiPolygon)");

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
    }
}
