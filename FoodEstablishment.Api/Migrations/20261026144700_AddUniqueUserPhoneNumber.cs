using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodEstablishment.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueUserPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_users_phonenumber",
                table: "users",
                column: "phonenumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_phonenumber",
                table: "users");
        }
    }
}