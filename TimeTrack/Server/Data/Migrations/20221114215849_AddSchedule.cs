using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTrack.Server.Data.Migrations
{
    public partial class AddSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndSchedule = table.Column<DateTime>(type: "date", nullable: false, comment: "Date schedule should terminte on, inclusive"),
                    DaysOfWeek = table.Column<byte>(type: "tinyint", nullable: false, comment: "Bits for day of week Sunday to Saturday starting at the most significant bit."),
                    Interval = table.Column<int>(type: "int", nullable: false, comment: "Gap between events. e.g 2 could be for every othe week"),
                    Frequency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ScheduleId",
                table: "Activities",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Schedule_ScheduleId",
                table: "Activities",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Schedule_ScheduleId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ScheduleId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Activities");
        }
    }
}
