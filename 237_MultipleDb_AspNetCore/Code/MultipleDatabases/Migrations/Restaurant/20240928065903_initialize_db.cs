using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultipleDatabases.Migrations.Restaurant
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
                values: new object[] { 1, "Burgers" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
