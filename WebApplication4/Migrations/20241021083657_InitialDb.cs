using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication4.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Manufacturer = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Color", "Manufacturer", "Model", "Price" },
                values: new object[,]
                {
                    { 1, 1, 0, "Malibu", 25000m },
                    { 2, 0, 0, "Tracker", 20000m },
                    { 3, 1, 1, "X6", 50000m },
                    { 4, 2, 8, "Camry", 12000m },
                    { 5, 3, 3, "Civic", 10000m },
                    { 6, 4, 5, "Focus", 13000m },
                    { 7, 0, 1, "X5", 35000m },
                    { 8, 7, 11, "A4", 30000m },
                    { 9, 1, 2, "C-Class", 20000m },
                    { 10, 1, 6, "Elantra", 25000m },
                    { 11, 2, 4, "Optima", 15000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
