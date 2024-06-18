using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollDancingLibrary.Migrations
{
    /// <inheritdoc />
    public partial class LegislationCongress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                  name: "Congress",
                  table: "Legislations",
                  newName: "CongressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CongressId",
                table: "Legislations",
                newName: "Congress");
        }
    }
}
