using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeedDatabase.Migrations.Weed
{
    public partial class ImprooveWeedItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "strain",
                schema: "store",
                table: "weed",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "thc",
                schema: "store",
                table: "weed",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "strain",
                schema: "store",
                table: "weed");

            migrationBuilder.DropColumn(
                name: "thc",
                schema: "store",
                table: "weed");
        }
    }
}
