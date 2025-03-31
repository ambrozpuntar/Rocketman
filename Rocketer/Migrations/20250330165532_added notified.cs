using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocketer.Migrations
{
    /// <inheritdoc />
    public partial class Addednotified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotifiedStatus",
                table: "Launches",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifiedStatus",
                table: "Launches");
        }
    }
}
