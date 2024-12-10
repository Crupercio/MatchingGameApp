using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MatchingGameApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimeCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GameItems",
                columns: new[] { "Id", "Category", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 17, "Anime", "/images/goku.png", "Goku" },
                    { 18, "Anime", "/images/Chichi.png", "Chichi" },
                    { 19, "Anime", "/images/Bills.png", "Bills" },
                    { 20, "Anime", "/images/Puar.png", "Puar" },
                    { 21, "Anime", "/images/Broly.png", "Broly" },
                    { 22, "Anime", "/images/Bulma.png", "Bulma" },
                    { 23, "Anime", "/images/Trunks.png", "Trunks" },
                    { 24, "Anime", "/images/Krillin.png", "Krillin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 24);
        }
    }
}
