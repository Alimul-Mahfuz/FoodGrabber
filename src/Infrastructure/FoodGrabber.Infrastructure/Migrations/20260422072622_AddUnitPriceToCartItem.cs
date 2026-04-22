using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodGrabber.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitPriceToCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "unit_price",
                table: "CartItems",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit_price",
                table: "CartItems");
        }
    }
}
