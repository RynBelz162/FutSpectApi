using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutSpect.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ClubLogoExtensionUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "ClubLogos",
                type: "character varying(25)",
                unicode: false,
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ScrapeLedger_LeagueId",
                table: "ScrapeLedger",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScrapeLedger_Leagues_LeagueId",
                table: "ScrapeLedger",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScrapeLedger_Leagues_LeagueId",
                table: "ScrapeLedger");

            migrationBuilder.DropIndex(
                name: "IX_ScrapeLedger_LeagueId",
                table: "ScrapeLedger");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "ClubLogos");
        }
    }
}
