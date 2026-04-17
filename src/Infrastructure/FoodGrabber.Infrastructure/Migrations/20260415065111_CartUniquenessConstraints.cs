using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodGrabber.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CartUniquenessConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Carts_user_id",
                table: "Carts");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_user_id",
                table: "Carts",
                column: "user_id",
                unique: true,
                filter: "[user_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_cart_id_item_id_item_type",
                table: "CartItems",
                columns: new[] { "cart_id", "item_id", "item_type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Carts_user_id",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_cart_id_item_id_item_type",
                table: "CartItems");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_user_id",
                table: "Carts",
                column: "user_id");
        }
    }
}
