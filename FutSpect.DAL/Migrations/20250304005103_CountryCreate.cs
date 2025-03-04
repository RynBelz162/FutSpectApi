using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FutSpect.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CountryCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "Clubs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Clubs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClubLogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClubId = table.Column<int>(type: "integer", nullable: false),
                    Bytes = table.Column<byte[]>(type: "bytea", nullable: false),
                    SrcUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubLogos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false),
                    Abbreviation = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leagues_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_ClubId",
                table: "Clubs",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_CountryId",
                table: "Clubs",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_Name",
                table: "Clubs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_CountryId",
                table: "Leagues",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_Name_CountryId",
                table: "Leagues",
                columns: new[] { "Name", "CountryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_ClubLogos_ClubId",
                table: "Clubs",
                column: "ClubId",
                principalTable: "ClubLogos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_Countries_CountryId",
                table: "Clubs",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_ClubLogos_ClubId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_Countries_CountryId",
                table: "Clubs");

            migrationBuilder.DropTable(
                name: "ClubLogos");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_ClubId",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_CountryId",
                table: "Clubs");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_Name",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Clubs");
        }
    }
}
