using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFCoreTips.Migrations
{
    /// <inheritdoc />
    public partial class insert_Tables : Migration
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

            migrationBuilder.CreateTable(
                name: "RawProductsResponse",
                columns: table => new
                {
                    productId = table.Column<int>(type: "INTEGER", nullable: false),
                    productName = table.Column<string>(type: "TEXT", nullable: false),
                    catId = table.Column<int>(type: "INTEGER", nullable: false),
                    isActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    productId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    productName = table.Column<string>(type: "TEXT", nullable: false),
                    catId = table.Column<int>(type: "INTEGER", nullable: false),
                    isActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.productId);
                    table.ForeignKey(
                        name: "FK_products_categories_catId",
                        column: x => x.catId,
                        principalTable: "categories",
                        principalColumn: "catId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "catId", "catName" },
                values: new object[,]
                {
                    { 1, "Burgers" },
                    { 2, "Pizza" },
                    { 3, "Pasta" },
                    { 4, "Drinks" }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "productId", "catId", "isActive", "productName" },
                values: new object[,]
                {
                    { 1, 2, true, "Product 0" },
                    { 2, 2, false, "Product 1" },
                    { 3, 2, true, "Product 2" },
                    { 4, 2, false, "Product 3" },
                    { 5, 2, true, "Product 4" },
                    { 6, 2, false, "Product 5" },
                    { 7, 2, true, "Product 6" },
                    { 8, 2, false, "Product 7" },
                    { 9, 2, true, "Product 8" },
                    { 10, 2, false, "Product 9" },
                    { 11, 2, true, "Product 10" },
                    { 12, 2, false, "Product 11" },
                    { 13, 2, true, "Product 12" },
                    { 14, 2, false, "Product 13" },
                    { 15, 2, true, "Product 14" },
                    { 16, 2, false, "Product 15" },
                    { 17, 2, true, "Product 16" },
                    { 18, 2, false, "Product 17" },
                    { 19, 2, true, "Product 18" },
                    { 20, 2, false, "Product 19" },
                    { 21, 2, true, "Product 20" },
                    { 22, 2, false, "Product 21" },
                    { 23, 2, true, "Product 22" },
                    { 24, 2, false, "Product 23" },
                    { 25, 2, true, "Product 24" },
                    { 26, 2, false, "Product 25" },
                    { 27, 2, true, "Product 26" },
                    { 28, 2, false, "Product 27" },
                    { 29, 2, true, "Product 28" },
                    { 30, 2, false, "Product 29" },
                    { 31, 2, true, "Product 30" },
                    { 32, 2, false, "Product 31" },
                    { 33, 2, true, "Product 32" },
                    { 34, 2, false, "Product 33" },
                    { 35, 2, true, "Product 34" },
                    { 36, 2, false, "Product 35" },
                    { 37, 2, true, "Product 36" },
                    { 38, 2, false, "Product 37" },
                    { 39, 2, true, "Product 38" },
                    { 40, 2, false, "Product 39" },
                    { 41, 2, true, "Product 40" },
                    { 42, 2, false, "Product 41" },
                    { 43, 2, true, "Product 42" },
                    { 44, 2, false, "Product 43" },
                    { 45, 2, true, "Product 44" },
                    { 46, 2, false, "Product 45" },
                    { 47, 2, true, "Product 46" },
                    { 48, 2, false, "Product 47" },
                    { 49, 2, true, "Product 48" },
                    { 50, 2, false, "Product 49" },
                    { 51, 2, true, "Product 50" },
                    { 52, 2, false, "Product 51" },
                    { 53, 2, true, "Product 52" },
                    { 54, 2, false, "Product 53" },
                    { 55, 2, true, "Product 54" },
                    { 56, 2, false, "Product 55" },
                    { 57, 2, true, "Product 56" },
                    { 58, 2, false, "Product 57" },
                    { 59, 2, true, "Product 58" },
                    { 60, 2, false, "Product 59" },
                    { 61, 2, true, "Product 60" },
                    { 62, 2, false, "Product 61" },
                    { 63, 2, true, "Product 62" },
                    { 64, 2, false, "Product 63" },
                    { 65, 2, true, "Product 64" },
                    { 66, 2, false, "Product 65" },
                    { 67, 2, true, "Product 66" },
                    { 68, 2, false, "Product 67" },
                    { 69, 2, true, "Product 68" },
                    { 70, 2, false, "Product 69" },
                    { 71, 2, true, "Product 70" },
                    { 72, 2, false, "Product 71" },
                    { 73, 2, true, "Product 72" },
                    { 74, 2, false, "Product 73" },
                    { 75, 2, true, "Product 74" },
                    { 76, 2, false, "Product 75" },
                    { 77, 2, true, "Product 76" },
                    { 78, 2, false, "Product 77" },
                    { 79, 2, true, "Product 78" },
                    { 80, 2, false, "Product 79" },
                    { 81, 2, true, "Product 80" },
                    { 82, 2, false, "Product 81" },
                    { 83, 2, true, "Product 82" },
                    { 84, 2, false, "Product 83" },
                    { 85, 2, true, "Product 84" },
                    { 86, 2, false, "Product 85" },
                    { 87, 2, true, "Product 86" },
                    { 88, 2, false, "Product 87" },
                    { 89, 2, true, "Product 88" },
                    { 90, 2, false, "Product 89" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_catId",
                table: "products",
                column: "catId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "RawProductsResponse");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
