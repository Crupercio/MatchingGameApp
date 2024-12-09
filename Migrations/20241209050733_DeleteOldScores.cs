using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchingGameApp.Migrations
{
    /// <inheritdoc />
    public partial class DeleteOldScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Scores");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
