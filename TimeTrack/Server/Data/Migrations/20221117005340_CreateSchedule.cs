using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTrack.Server.Data.Migrations
{
    public partial class CreateSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Schedule_ScheduleId",
                table: "Activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule");

            migrationBuilder.RenameTable(
                name: "Schedule",
                newName: "Schedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Schedules_ScheduleId",
                table: "Activities",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Schedules_ScheduleId",
                table: "Activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "Schedule");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Schedule_ScheduleId",
                table: "Activities",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id");
        }
    }
}
