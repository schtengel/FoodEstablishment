using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodEstablishment.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddNewOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "orderstatuses",
                columns: new[] { "id", "createdat", "isdeleted", "name", "updatedat" },
                values: new object[] { 5, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Отдан", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "orderstatuses",
                keyColumn: "id",
                keyValue: 5);
        }
    }
}
