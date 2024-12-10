using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchingGameApp.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove the AddColumn operation since the column already exists
            // migrationBuilder.AddColumn<string>(
            //     name: "Category",
            //     table: "Scores",
            //     type: "TEXT",
            //     nullable: false,
            //     defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the DropColumn operation since we are not adding it
            // migrationBuilder.DropColumn(
            //     name: "Category",
            //     table: "Scores");
        }
    }
}

