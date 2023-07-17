using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeedDatabase.Migrations.Weed
{
    public partial class AddMainDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "items");

            migrationBuilder.EnsureSchema(
                name: "store");

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "store",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    delivery = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "weed",
                schema: "store",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    unqiue = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    descriptions = table.Column<string>(type: "text", nullable: false),
                    photo_url = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weed", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    weed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_orders_order_id",
                        column: x => x.order_id,
                        principalSchema: "store",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_id1",
                schema: "items",
                table: "orders",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_order_id",
                schema: "items",
                table: "orders",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_weed_id",
                schema: "items",
                table: "orders",
                column: "weed_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_id",
                schema: "store",
                table: "orders",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_phone_number",
                schema: "store",
                table: "orders",
                column: "phone_number");

            migrationBuilder.CreateIndex(
                name: "IX_weed_id",
                schema: "store",
                table: "weed",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_weed_name",
                schema: "store",
                table: "weed",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_weed_unqiue",
                schema: "store",
                table: "weed",
                column: "unqiue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders",
                schema: "items");

            migrationBuilder.DropTable(
                name: "weed",
                schema: "store");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "store");
        }
    }
}
