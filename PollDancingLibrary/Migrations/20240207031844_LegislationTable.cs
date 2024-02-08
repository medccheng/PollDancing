using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollDancingLibrary.Migrations
{
    /// <inheritdoc />
    public partial class LegislationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressInformationId",
                table: "Members",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Legislations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Congress = table.Column<int>(type: "int", nullable: false),
                    IntroducedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legislations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CosponsoredLegislation",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    LegislationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosponsoredLegislation", x => new { x.MemberId, x.LegislationId });
                    table.ForeignKey(
                        name: "FK_CosponsoredLegislation_Legislations_LegislationId",
                        column: x => x.LegislationId,
                        principalTable: "Legislations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CosponsoredLegislation_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SponsoredLegislations",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    LegislationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SponsoredLegislations", x => new { x.MemberId, x.LegislationId });
                    table.ForeignKey(
                        name: "FK_SponsoredLegislations_Legislations_LegislationId",
                        column: x => x.LegislationId,
                        principalTable: "Legislations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SponsoredLegislations_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_AddressInformationId",
                table: "Members",
                column: "AddressInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_CosponsoredLegislation_LegislationId",
                table: "CosponsoredLegislation",
                column: "LegislationId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsoredLegislations_LegislationId",
                table: "SponsoredLegislations",
                column: "LegislationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_AddressInformations_AddressInformationId",
                table: "Members",
                column: "AddressInformationId",
                principalTable: "AddressInformations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_AddressInformations_AddressInformationId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "CosponsoredLegislation");

            migrationBuilder.DropTable(
                name: "SponsoredLegislations");

            migrationBuilder.DropTable(
                name: "Legislations");

            migrationBuilder.DropIndex(
                name: "IX_Members_AddressInformationId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AddressInformationId",
                table: "Members");
        }
    }
}
