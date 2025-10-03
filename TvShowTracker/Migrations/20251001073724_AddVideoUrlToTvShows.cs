using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvShowTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoUrlToTvShows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "TvShows",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "TvShows");
        }
    }
}
