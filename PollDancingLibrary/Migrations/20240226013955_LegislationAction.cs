using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollDancingLibrary.Migrations
{
    /// <inheritdoc />
    public partial class LegislationAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConstitutionalAuthorityStatementText",
                table: "Legislations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginChamber",
                table: "Legislations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginChamberCode",
                table: "Legislations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PolicyAreaId",
                table: "Legislations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Legislations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDateIncludingText",
                table: "Legislations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegislationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Actions_Legislations_LegislationId",
                        column: x => x.LegislationId,
                        principalTable: "Legislations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PolicyAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyAreas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Legislations_PolicyAreaId",
                table: "Legislations",
                column: "PolicyAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_LegislationId",
                table: "Actions",
                column: "LegislationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Legislations_PolicyAreas_PolicyAreaId",
                table: "Legislations",
                column: "PolicyAreaId",
                principalTable: "PolicyAreas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Legislations_PolicyAreas_PolicyAreaId",
                table: "Legislations");

            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "PolicyAreas");

            migrationBuilder.DropIndex(
                name: "IX_Legislations_PolicyAreaId",
                table: "Legislations");

            migrationBuilder.DropColumn(
                name: "ConstitutionalAuthorityStatementText",
                table: "Legislations");

            migrationBuilder.DropColumn(
                name: "OriginChamber",
                table: "Legislations");

            migrationBuilder.DropColumn(
                name: "OriginChamberCode",
                table: "Legislations");

            migrationBuilder.DropColumn(
                name: "PolicyAreaId",
                table: "Legislations");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Legislations");

            migrationBuilder.DropColumn(
                name: "UpdateDateIncludingText",
                table: "Legislations");
        }
    }
}
