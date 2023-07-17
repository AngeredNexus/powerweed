using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeedDatabase.Migrations
{
    public partial class AddTelegramDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "telegram");

            migrationBuilder.CreateTable(
                name: "bot_user_map",
                schema: "telegram",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bot_type = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    lang = table.Column<int>(type: "integer", nullable: false),
                    has_accepted_law_policy = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bot_user_map", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bot_user_map_bot_type",
                schema: "telegram",
                table: "bot_user_map",
                column: "bot_type");

            migrationBuilder.CreateIndex(
                name: "IX_bot_user_map_id",
                schema: "telegram",
                table: "bot_user_map",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_bot_user_map_user_id",
                schema: "telegram",
                table: "bot_user_map",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bot_user_map",
                schema: "telegram");
        }
    }
}
