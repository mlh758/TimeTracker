using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTrack.Server.Data.Migrations
{
    public partial class AddGroupingToActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ActivityGroupingId",
                table: "Activities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityGroupingId",
                table: "Activities",
                column: "ActivityGroupingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ActivityGrouping_ActivityGroupingId",
                table: "Activities",
                column: "ActivityGroupingId",
                principalTable: "ActivityGrouping",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ActivityGrouping_ActivityGroupingId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ActivityGroupingId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ActivityGroupingId",
                table: "Activities");
        }
    }
}
