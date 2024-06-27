using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TucaAPI.Migrations
{
    /// <inheritdoc />
    public partial class addTwoFactorSecret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ddfb37a-4508-412d-a74e-4758a460465d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6db21300-704c-4359-938c-e377f6c289ff");

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorSecret",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d476825b-cbf8-4412-b048-20d112e940f3", null, "Admin", "Admin" },
                    { "fba69d46-cd31-450e-b77a-063e5645b6d3", null, "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d476825b-cbf8-4412-b048-20d112e940f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fba69d46-cd31-450e-b77a-063e5645b6d3");

            migrationBuilder.DropColumn(
                name: "TwoFactorSecret",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5ddfb37a-4508-412d-a74e-4758a460465d", null, "User", "USER" },
                    { "6db21300-704c-4359-938c-e377f6c289ff", null, "Admin", "ADMIN" }
                });
        }
    }
}
