using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodGrabber.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncAppDbContextModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "category_id",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "branch_id",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "dining_table_id",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "discount_amount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "service_type",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "subtotal_amount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "tax_amount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "item_name_snapshot",
                table: "OrderDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "price_source",
                table: "OrderDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "branch_id",
                table: "Menus",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MenuCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemModifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_details_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    modifier_group_name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    modifier_option_name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    unit_price_delta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_price_delta = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemModifiers_OrderDetails_order_details_id",
                        column: x => x.order_details_id,
                        principalTable: "OrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    previous_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    current_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_by = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistory_Orders_order_id",
                        column: x => x.order_id,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAvailabilityWindows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    day_of_week = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    is_available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAvailabilityWindows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAvailabilityWindows_Products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductModifierGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    min_selections = table.Column<int>(type: "int", nullable: false),
                    max_selections = table.Column<int>(type: "int", nullable: false),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductModifierGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductModifierGroups_Products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPriceHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    base_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    selling_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    effective_from = table.Column<DateTime>(type: "datetime2", nullable: false),
                    effective_to = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPriceHistories_Products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    legal_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    tax_registration_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuCategoryProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    menu_category_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategoryProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuCategoryProducts_MenuCategories_menu_category_id",
                        column: x => x.menu_category_id,
                        principalTable: "MenuCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductModifierOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_modifier_group_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    price_delta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    is_default = table.Column<bool>(type: "bit", nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductModifierOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductModifierOptions_ProductModifierGroups_product_modifier_group_id",
                        column: x => x.product_modifier_group_id,
                        principalTable: "ProductModifierGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    restaurant_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    address_line_1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    address_line_2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    city = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    state = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    postal_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    country_code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    time_zone = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Restaurants_restaurant_id",
                        column: x => x.restaurant_id,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiningTables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    branch_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    table_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    floor_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    section_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    seat_count = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiningTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiningTables_Branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_category_id_is_active",
                table: "Products",
                columns: new[] { "category_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_branch_id_created_at",
                table: "Orders",
                columns: new[] { "branch_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_created_at",
                table: "Orders",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_dining_table_id",
                table: "Orders",
                column: "dining_table_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_status_created_at",
                table: "Orders",
                columns: new[] { "status", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_branch_id_name",
                table: "Menus",
                columns: new[] { "branch_id", "name" });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_restaurant_id_name",
                table: "Branches",
                columns: new[] { "restaurant_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiningTables_branch_id_table_code",
                table: "DiningTables",
                columns: new[] { "branch_id", "table_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategories_branch_id_name",
                table: "MenuCategories",
                columns: new[] { "branch_id", "name" },
                unique: true,
                filter: "[branch_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategoryProducts_menu_category_id_product_id",
                table: "MenuCategoryProducts",
                columns: new[] { "menu_category_id", "product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategoryProducts_product_id",
                table: "MenuCategoryProducts",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemModifiers_order_details_id",
                table: "OrderItemModifiers",
                column: "order_details_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistory_order_id_changed_at",
                table: "OrderStatusHistory",
                columns: new[] { "order_id", "changed_at" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAvailabilityWindows_product_id_day_of_week_start_time_end_time",
                table: "ProductAvailabilityWindows",
                columns: new[] { "product_id", "day_of_week", "start_time", "end_time" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_name",
                table: "ProductCategories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductModifierGroups_product_id_name",
                table: "ProductModifierGroups",
                columns: new[] { "product_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductModifierOptions_product_modifier_group_id_name",
                table: "ProductModifierOptions",
                columns: new[] { "product_modifier_group_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPriceHistories_product_id_effective_from",
                table: "ProductPriceHistories",
                columns: new[] { "product_id", "effective_from" });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_name",
                table: "Restaurants",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branches_branch_id",
                table: "Orders",
                column: "branch_id",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DiningTables_dining_table_id",
                table: "Orders",
                column: "dining_table_id",
                principalTable: "DiningTables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategories_category_id",
                table: "Products",
                column: "category_id",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_branch_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DiningTables_dining_table_id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategories_category_id",
                table: "Products");

            migrationBuilder.DropTable(
                name: "DiningTables");

            migrationBuilder.DropTable(
                name: "MenuCategoryProducts");

            migrationBuilder.DropTable(
                name: "OrderItemModifiers");

            migrationBuilder.DropTable(
                name: "OrderStatusHistory");

            migrationBuilder.DropTable(
                name: "ProductAvailabilityWindows");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductModifierOptions");

            migrationBuilder.DropTable(
                name: "ProductPriceHistories");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "MenuCategories");

            migrationBuilder.DropTable(
                name: "ProductModifierGroups");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Products_category_id_is_active",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Orders_branch_id_created_at",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_created_at",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_dining_table_id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_status_created_at",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Menus_branch_id_name",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "branch_id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "dining_table_id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "discount_amount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "service_type",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "subtotal_amount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "tax_amount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "item_name_snapshot",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "price_source",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "branch_id",
                table: "Menus");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
