using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeedDatabase.Migrations.Weed
{
    public partial class UpdateDbStruct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "source",
                schema: "common",
                table: "users",
                newName: "messenger_source");

            migrationBuilder.AddColumn<string>(
                name: "auth_code",
                schema: "common",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "auth_code",
                schema: "common",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "messenger_source",
                schema: "common",
                table: "users",
                newName: "source");
        }
    }
}
