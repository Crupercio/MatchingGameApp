using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchingGameApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Scores",
                type: "TEXT",
                nullable: false,
                defaultValue: "Default");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Scores");
        }

    }
}
