using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutSpect.DAL.Migrations
{
    /// <inheritdoc />
    public partial class LeagueLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Leagues",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Leagues",
                type: "character varying(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LeagueLogos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LeagueId = table.Column<int>(type: "integer", nullable: false),
                    Bytes = table.Column<byte[]>(type: "bytea", nullable: false),
                    SrcUrl = table.Column<string>(type: "text", nullable: true),
                    Extension = table.Column<string>(type: "character varying(25)", unicode: false, maxLength: 25, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueLogos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeagueLogos_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueLogos_LeagueId",
                table: "LeagueLogos",
                column: "LeagueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueLogos");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Leagues");
        }
    }
}
