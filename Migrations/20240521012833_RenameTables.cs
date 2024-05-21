using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TucaAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d3d4e7f-3125-444b-ae9e-eb874c8e9dab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e3fbf9d-940b-4f22-9783-51328fb2f4ab");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cb2a19a3-8537-4d77-acbc-ca004bb4d330", null, "Admin", "ADMIN" },
                    { "e10af92f-1a9d-4597-92f9-447cfa74fd61", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb2a19a3-8537-4d77-acbc-ca004bb4d330");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e10af92f-1a9d-4597-92f9-447cfa74fd61");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0d3d4e7f-3125-444b-ae9e-eb874c8e9dab", null, "Admin", "ADMIN" },
                    { "2e3fbf9d-940b-4f22-9783-51328fb2f4ab", null, "User", "USER" }
                });
        }
    }
}
