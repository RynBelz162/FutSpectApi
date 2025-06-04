using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutSpect.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddModifiedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Leagues",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "LeagueLogos",
                newName: "ModifiedOn");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Leagues",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "LeagueLogos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "LeagueLogos");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "Leagues",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "LeagueLogos",
                newName: "CreatedDate");
        }
    }
}
