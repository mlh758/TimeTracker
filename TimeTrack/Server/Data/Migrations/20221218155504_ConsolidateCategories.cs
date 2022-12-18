using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeTrack.Server.Data.Migrations
{
    public partial class ConsolidateCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_CustomCategories_CustomAgeId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_CustomCategories_CustomGenderId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_CustomCategories_CustomRaceId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_CustomCategories_CustomSettingId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_CustomCategories_CustomSexualOrientationId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "ClientCustomDisability");

            migrationBuilder.DropTable(
                name: "CustomCategories");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CustomAgeId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CustomGenderId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CustomRaceId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CustomSettingId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CustomSexualOrientationId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CustomAgeId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CustomGenderId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CustomRaceId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CustomSettingId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CustomSexualOrientationId",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_UserId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Categories");

            migrationBuilder.AddColumn<long>(
                name: "CustomAgeId",
                table: "Clients",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomGenderId",
                table: "Clients",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomRaceId",
                table: "Clients",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomSettingId",
                table: "Clients",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomSexualOrientationId",
                table: "Clients",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomCategories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientCustomDisability",
                columns: table => new
                {
                    DisabilityId = table.Column<long>(type: "bigint", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCustomDisability", x => new { x.DisabilityId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_ClientCustomDisability_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientCustomDisability_CustomCategories_DisabilityId",
                        column: x => x.DisabilityId,
                        principalTable: "CustomCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CustomAgeId",
                table: "Clients",
                column: "CustomAgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CustomGenderId",
                table: "Clients",
                column: "CustomGenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CustomRaceId",
                table: "Clients",
                column: "CustomRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CustomSettingId",
                table: "Clients",
                column: "CustomSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CustomSexualOrientationId",
                table: "Clients",
                column: "CustomSexualOrientationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCustomDisability_ClientId",
                table: "ClientCustomDisability",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomCategories_UserId",
                table: "CustomCategories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_CustomCategories_CustomAgeId",
                table: "Clients",
                column: "CustomAgeId",
                principalTable: "CustomCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_CustomCategories_CustomGenderId",
                table: "Clients",
                column: "CustomGenderId",
                principalTable: "CustomCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_CustomCategories_CustomRaceId",
                table: "Clients",
                column: "CustomRaceId",
                principalTable: "CustomCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_CustomCategories_CustomSettingId",
                table: "Clients",
                column: "CustomSettingId",
                principalTable: "CustomCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_CustomCategories_CustomSexualOrientationId",
                table: "Clients",
                column: "CustomSexualOrientationId",
                principalTable: "CustomCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
