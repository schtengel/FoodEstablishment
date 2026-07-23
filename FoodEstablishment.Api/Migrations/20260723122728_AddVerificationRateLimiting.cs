using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodEstablishment.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddVerificationRateLimiting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "verificationattempts",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "verificationcodesentat",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "verificationlockeduntil",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "verificationattempts",
                table: "users");

            migrationBuilder.DropColumn(
                name: "verificationcodesentat",
                table: "users");

            migrationBuilder.DropColumn(
                name: "verificationlockeduntil",
                table: "users");
        }
    }
}
