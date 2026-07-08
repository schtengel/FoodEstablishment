using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodEstablishment.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedOrderAndPaymentStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "orderstatuses",
                columns: new[] { "id", "createdat", "isdeleted", "name", "updatedat" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Создан", null },
                    { 2, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "В процессе", null },
                    { 3, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Готов", null },
                    { 4, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Отменен", null }
                });

            migrationBuilder.InsertData(
                table: "paymentstatuses",
                columns: new[] { "id", "createdat", "isdeleted", "name", "updatedat" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Создан", null },
                    { 2, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "В процессе оплаты", null },
                    { 3, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Оплачен", null },
                    { 4, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Недостаточно средств", null },
                    { 5, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Отменен", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "orderstatuses",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "orderstatuses",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "orderstatuses",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "orderstatuses",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "paymentstatuses",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "paymentstatuses",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "paymentstatuses",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "paymentstatuses",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "paymentstatuses",
                keyColumn: "id",
                keyValue: 5);
        }
    }
}
