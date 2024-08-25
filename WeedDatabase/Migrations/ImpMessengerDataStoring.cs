using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class ImpMessengerDataStoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "value",
                table: "auth_contact");

            migrationBuilder.AddColumn<Guid>(
                name: "dataId",
                table: "auth_contact",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "telegram_contacts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_id = table.Column<string>(type: "text", nullable: false),
                    user = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telegram_contacts", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "telegram_contacts");

            migrationBuilder.DropColumn(
                name: "dataId",
                table: "auth_contact");

            migrationBuilder.AddColumn<string>(
                name: "value",
                table: "auth_contact",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
