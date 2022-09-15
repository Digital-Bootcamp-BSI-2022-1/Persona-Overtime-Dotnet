using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persona.Migrations
{
    public partial class edituserorganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organization_User_headid",
                table: "Organization");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_User_memberid",
                table: "Organization");

            migrationBuilder.DropIndex(
                name: "IX_Organization_headid",
                table: "Organization");

            migrationBuilder.DropIndex(
                name: "IX_Organization_memberid",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "headid",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "memberid",
                table: "Organization");

            migrationBuilder.AddColumn<int>(
                name: "organizationid",
                table: "User",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_organizationid",
                table: "User",
                column: "organizationid");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Organization_organizationid",
                table: "User",
                column: "organizationid",
                principalTable: "Organization",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Organization_organizationid",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_organizationid",
                table: "User");

            migrationBuilder.DropColumn(
                name: "organizationid",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "headid",
                table: "Organization",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "memberid",
                table: "Organization",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Organization_headid",
                table: "Organization",
                column: "headid");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_memberid",
                table: "Organization",
                column: "memberid");

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_User_headid",
                table: "Organization",
                column: "headid",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_User_memberid",
                table: "Organization",
                column: "memberid",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
