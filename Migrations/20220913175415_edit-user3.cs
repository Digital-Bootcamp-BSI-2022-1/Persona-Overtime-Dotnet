using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persona.Migrations
{
    public partial class edituser3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_head",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_head",
                table: "User");
        }
    }
}
