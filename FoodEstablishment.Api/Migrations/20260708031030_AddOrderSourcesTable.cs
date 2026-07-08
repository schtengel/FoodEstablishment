using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodEstablishment.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderSourcesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ordersources",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordersources", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "ordersources",
                columns: new[] { "id", "createdat", "isdeleted", "name", "updatedat" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Мобильное приложение", null },
                    { 2, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Терминал самообслуживания", null },
                    { 3, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Касса", null },
                    { 4, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Веб-сайт", null },
                    { 5, new DateTime(2026, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, "Другое", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ordersources");
        }
    }
}
