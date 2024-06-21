using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollDancingLibrary.Migrations
{
    /// <inheritdoc />
    public partial class BillUpdateFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedsUpdate",
                table: "Legislations",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedsUpdate",
                table: "Legislations");
        }
    }
}
