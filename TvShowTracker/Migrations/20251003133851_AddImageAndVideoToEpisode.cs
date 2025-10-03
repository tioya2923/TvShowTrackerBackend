using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvShowTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddImageAndVideoToEpisode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Episodes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Episodes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Episodes");
        }
    }
}
