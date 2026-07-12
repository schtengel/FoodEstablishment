using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodEstablishment.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderAndOrderComposition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_users_userid",
                table: "order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order",
                table: "order");

            migrationBuilder.RenameTable(
                name: "order",
                newName: "orders");

            migrationBuilder.RenameIndex(
                name: "IX_order_userid",
                table: "orders",
                newName: "IX_orders_userid");

            migrationBuilder.AlterColumn<int>(
                name: "userid",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ordersourceid",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "orderstatusid",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_orders",
                table: "orders",
                column: "id");

            migrationBuilder.CreateTable(
                name: "ordercompositions",
                columns: table => new
                {
                    orderid = table.Column<int>(type: "integer", nullable: false),
                    productid = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    priceatordertime = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordercompositions", x => new { x.orderid, x.productid });
                    table.ForeignKey(
                        name: "FK_ordercompositions_orders_orderid",
                        column: x => x.orderid,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ordercompositions_products_productid",
                        column: x => x.productid,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_ordersourceid",
                table: "orders",
                column: "ordersourceid");

            migrationBuilder.CreateIndex(
                name: "IX_orders_orderstatusid",
                table: "orders",
                column: "orderstatusid");

            migrationBuilder.CreateIndex(
                name: "IX_ordercompositions_productid",
                table: "ordercompositions",
                column: "productid");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_ordersources_ordersourceid",
                table: "orders",
                column: "ordersourceid",
                principalTable: "ordersources",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_orderstatuses_orderstatusid",
                table: "orders",
                column: "orderstatusid",
                principalTable: "orderstatuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_userid",
                table: "orders",
                column: "userid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_ordersources_ordersourceid",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_orderstatuses_orderstatusid",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_userid",
                table: "orders");

            migrationBuilder.DropTable(
                name: "ordercompositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orders",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_ordersourceid",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_orderstatusid",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "ordersourceid",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "orderstatusid",
                table: "orders");

            migrationBuilder.RenameTable(
                name: "orders",
                newName: "order");

            migrationBuilder.RenameIndex(
                name: "IX_orders_userid",
                table: "order",
                newName: "IX_order_userid");

            migrationBuilder.AlterColumn<int>(
                name: "userid",
                table: "order",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_order",
                table: "order",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_users_userid",
                table: "order",
                column: "userid",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
