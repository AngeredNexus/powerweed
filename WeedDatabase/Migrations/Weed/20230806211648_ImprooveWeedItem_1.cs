using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeedDatabase.Migrations.Weed
{
    public partial class ImprooveWeedItem_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "price",
                schema: "store",
                table: "weed",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                schema: "store",
                table: "weed");
        }
    }
}
