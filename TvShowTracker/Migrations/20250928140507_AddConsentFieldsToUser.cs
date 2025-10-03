using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TvShowTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddConsentFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConsentDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConsentGiven",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsentDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConsentGiven",
                table: "Users");
        }
    }
}
