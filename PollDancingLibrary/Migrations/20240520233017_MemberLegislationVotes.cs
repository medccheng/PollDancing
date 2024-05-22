using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollDancingLibrary.Migrations
{
    /// <inheritdoc />
    public partial class MemberLegislationVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberLegislationVotes",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    LegislationId = table.Column<int>(type: "int", nullable: false),
                    Vote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberLegislationVotes", x => new { x.MemberId, x.LegislationId });
                    table.ForeignKey(
                        name: "FK_MemberLegislationVotes_Legislations_LegislationId",
                        column: x => x.LegislationId,
                        principalTable: "Legislations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberLegislationVotes_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberLegislationVotes_LegislationId",
                table: "MemberLegislationVotes",
                column: "LegislationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberLegislationVotes");
        }
    }
}
