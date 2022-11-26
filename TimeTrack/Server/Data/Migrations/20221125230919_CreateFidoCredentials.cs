using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTrack.Server.Data.Migrations
{
    public partial class CreateFidoCredentials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PublicKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CredentialId = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    SignatureCounter = table.Column<long>(type: "bigint", nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AaGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCredentials_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCredentials_UserId",
                table: "UserCredentials",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCredentials");
        }
    }
}
