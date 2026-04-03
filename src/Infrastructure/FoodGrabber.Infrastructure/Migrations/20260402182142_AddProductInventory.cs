using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodGrabber.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "Products",
                newName: "current_stock");

            migrationBuilder.AlterColumn<decimal>(
                name: "current_stock",
                table: "Products",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "stock_unit",
                table: "Products",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "piece");

            migrationBuilder.CreateTable(
                name: "ProductStockEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    movement_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStockEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStockEntries_Products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStockEntries_product_id_created_at",
                table: "ProductStockEntries",
                columns: new[] { "product_id", "created_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductStockEntries");

            migrationBuilder.DropColumn(
                name: "stock_unit",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "current_stock",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");

            migrationBuilder.RenameColumn(
                name: "current_stock",
                table: "Products",
                newName: "quantity");
        }
    }
}
