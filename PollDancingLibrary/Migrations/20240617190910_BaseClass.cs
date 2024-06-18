using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollDancingLibrary.Migrations
{
    /// <inheritdoc />
    public partial class BaseClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrent",
                table: "Members");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Terms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Terms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Terms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Terms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SponsoredLegislations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "SponsoredLegislations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "SponsoredLegislations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SponsoredLegislations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Sessions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Sessions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PolicyAreas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PolicyAreas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "PolicyAreas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PolicyAreas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MemberLegislationVotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MemberLegislationVotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "MemberLegislationVotes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "MemberLegislationVotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Legislations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Legislations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Legislations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Depictions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Depictions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Depictions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Depictions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CosponsoredLegislation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "CosponsoredLegislation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "CosponsoredLegislation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CosponsoredLegislation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Congresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Congresses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Congresses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Congresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AddressInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AddressInformations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "AddressInformations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AddressInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Actions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Actions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Actions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Actions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScoreCards",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FocusArea = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RelatedActions = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreCards", x => new { x.MemberId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_ScoreCards_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCards_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCards_SubjectId",
                table: "ScoreCards",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreCards");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Terms");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Terms");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Terms");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Terms");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SponsoredLegislations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "SponsoredLegislations");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "SponsoredLegislations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SponsoredLegislations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PolicyAreas");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PolicyAreas");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "PolicyAreas");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PolicyAreas");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MemberLegislationVotes");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MemberLegislationVotes");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "MemberLegislationVotes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MemberLegislationVotes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Legislations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Legislations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Legislations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Depictions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Depictions");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Depictions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Depictions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CosponsoredLegislation");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "CosponsoredLegislation");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "CosponsoredLegislation");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CosponsoredLegislation");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Congresses");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AddressInformations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AddressInformations");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "AddressInformations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AddressInformations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Actions");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
