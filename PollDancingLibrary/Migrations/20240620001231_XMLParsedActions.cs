using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollDancingLibrary.Migrations
{
    /// <inheritdoc />
    public partial class XMLParsedActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsParsed",
                table: "Actions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsParsed",
                table: "Actions");
        }
    }
}
