using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvShowTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddEpisodeDescriptionAndOtherChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Episodes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Episodes");
        }
    }
}
