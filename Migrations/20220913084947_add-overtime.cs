using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persona.Migrations
{
    public partial class addovertime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Overtime",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    status = table.Column<string>(type: "varchar(30)", nullable: false),
                    is_completed = table.Column<int>(type: "int", nullable: false),
                    remarks = table.Column<string>(type: "varchar(200)", nullable: false),
                    attachment = table.Column<string>(type: "varchar(500)", nullable: false),
                    request_date = table.Column<DateOnly>(type: "date", nullable: false),
                    request_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    approved_date = table.Column<DateOnly>(type: "date", nullable: false),
                    approved_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    completed_date = table.Column<DateOnly>(type: "date", nullable: false),
                    completed_time = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Overtime", x => x.id);
                    table.ForeignKey(
                        name: "FK_Overtime_User_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Overtime_userid",
                table: "Overtime",
                column: "userid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Overtime");
        }
    }
}
