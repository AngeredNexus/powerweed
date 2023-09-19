using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeedDatabase.Migrations
{
    public partial class UpdateDbStruct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_bot_user_map_bot_type",
                schema: "telegram",
                table: "bot_user_map");

            migrationBuilder.RenameColumn(
                name: "bot_type",
                schema: "telegram",
                table: "bot_user_map",
                newName: "messenger");

            migrationBuilder.AddColumn<Guid>(
                name: "app_user_id",
                schema: "telegram",
                table: "bot_user_map",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "hash",
                schema: "telegram",
                table: "bot_user_map",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_bot_user_map_hash",
                schema: "telegram",
                table: "bot_user_map",
                column: "hash",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_bot_user_map_hash",
                schema: "telegram",
                table: "bot_user_map");

            migrationBuilder.DropColumn(
                name: "app_user_id",
                schema: "telegram",
                table: "bot_user_map");

            migrationBuilder.DropColumn(
                name: "hash",
                schema: "telegram",
                table: "bot_user_map");

            migrationBuilder.RenameColumn(
                name: "messenger",
                schema: "telegram",
                table: "bot_user_map",
                newName: "bot_type");

            migrationBuilder.CreateIndex(
                name: "IX_bot_user_map_bot_type",
                schema: "telegram",
                table: "bot_user_map",
                column: "bot_type");
        }
    }
}
