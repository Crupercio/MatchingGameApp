using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MatchingGameApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdditionalGameItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GameItems",
                columns: new[] { "Id", "Category", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 5, "Animals", "/images/rabbit.png", "Rabbit" },
                    { 6, "Animals", "/images/horse.png", "Horse" },
                    { 7, "Animals", "/images/elephant.png", "Elephant" },
                    { 8, "Animals", "/images/lion.png", "Lion" },
                    { 9, "Animals", "/images/tiger.png", "Tiger" },
                    { 10, "Animals", "/images/bear.png", "Bear" },
                    { 11, "Fruits", "/images/cherry.png", "Cherry" },
                    { 12, "Fruits", "/images/grapes.png", "Grapes" },
                    { 13, "Fruits", "/images/orange.png", "Orange" },
                    { 14, "Fruits", "/images/pineapple.webp", "Pineapple" },
                    { 15, "Fruits", "/images/strawberry.webp", "Strawberry" },
                    { 16, "Fruits", "/images/watermelon.webp", "Watermelon" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "GameItems",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
