using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InMemoryCaching.Migrations
{
    /// <inheritdoc />
    public partial class initialize_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    catId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    catName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.catId);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "catId", "catName" },
                values: new object[,]
                {
                    { 1, "Burgers 0" },
                    { 2, "Burgers 1" },
                    { 3, "Burgers 2" },
                    { 4, "Burgers 3" },
                    { 5, "Burgers 4" },
                    { 6, "Burgers 5" },
                    { 7, "Burgers 6" },
                    { 8, "Burgers 7" },
                    { 9, "Burgers 8" },
                    { 10, "Burgers 9" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
