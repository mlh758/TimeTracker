using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTrack.Server.Data.Migrations
{
    public partial class DurationToClinicalHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Activities");

            migrationBuilder.AddColumn<decimal>(
                name: "ClinicalHours",
                table: "Activities",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicalHours",
                table: "Activities");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Duration in minutes");
        }
    }
}
