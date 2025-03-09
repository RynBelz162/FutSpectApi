using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FutSpect.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ScrapeLedgerCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScrapeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScrapeLedgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    LeagueId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapeLedgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScrapeLedgers_ScrapeTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ScrapeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScrapeLedgers_TypeId",
                table: "ScrapeLedgers",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrapeTypes_Name",
                table: "ScrapeTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScrapeLedgers");

            migrationBuilder.DropTable(
                name: "ScrapeTypes");
        }
    }
}
