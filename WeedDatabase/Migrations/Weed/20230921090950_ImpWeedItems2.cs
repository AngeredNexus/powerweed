using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeedDatabase.Migrations.Weed
{
    public partial class ImpWeedItems2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_has_pcs",
                schema: "store",
                table: "weed",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "pcs",
                schema: "store",
                table: "weed",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_has_pcs",
                schema: "store",
                table: "weed");

            migrationBuilder.DropColumn(
                name: "pcs",
                schema: "store",
                table: "weed");
        }
    }
}
