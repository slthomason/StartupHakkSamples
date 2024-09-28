using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultipleDatabases.Migrations
{
    /// <inheritdoc />
    public partial class initialize_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    customerName = table.Column<string>(type: "TEXT", nullable: false),
                    phoneNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userId);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "userId", "customerName", "phoneNumber" },
                values: new object[] { 1, "Jon Doe", "+168025600884" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
