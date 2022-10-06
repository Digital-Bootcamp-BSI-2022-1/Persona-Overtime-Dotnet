using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persona.Migrations
{
    public partial class editorganizations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "headid",
                table: "Organization",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organization_headid",
                table: "Organization",
                column: "headid");

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_User_headid",
                table: "Organization",
                column: "headid",
                principalTable: "User",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organization_User_headid",
                table: "Organization");

            migrationBuilder.DropIndex(
                name: "IX_Organization_headid",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "headid",
                table: "Organization");
        }
    }
}
