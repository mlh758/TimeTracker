using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTrack.Server.Data.Migrations
{
    public partial class ImproveDeleteOnActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Clients_ClientId",
                table: "Activities");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Clients_ClientId",
                table: "Activities",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Clients_ClientId",
                table: "Activities");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Clients_ClientId",
                table: "Activities",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
