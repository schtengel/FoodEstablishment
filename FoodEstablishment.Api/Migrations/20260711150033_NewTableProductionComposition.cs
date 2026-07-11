using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodEstablishment.Api.Migrations
{
    /// <inheritdoc />
    public partial class NewTableProductionComposition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "productcompositions",
                columns: table => new
                {
                    productid = table.Column<int>(type: "integer", nullable: false),
                    ingredientid = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productcompositions", x => new { x.productid, x.ingredientid });
                    table.ForeignKey(
                        name: "FK_productcompositions_ingredients_ingredientid",
                        column: x => x.ingredientid,
                        principalTable: "ingredients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_productcompositions_products_productid",
                        column: x => x.productid,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_productcompositions_ingredientid",
                table: "productcompositions",
                column: "ingredientid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productcompositions");
        }
    }
}
