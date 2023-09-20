using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeedDatabase.Migrations.Weed
{
    public partial class ImpOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "app_user_id",
                schema: "store",
                table: "orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "app_user_id",
                schema: "store",
                table: "orders");
        }
    }
}
