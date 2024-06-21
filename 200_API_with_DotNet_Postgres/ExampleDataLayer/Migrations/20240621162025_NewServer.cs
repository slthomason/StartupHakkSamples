using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExampleDataLayer.Migrations
{
    /// <inheritdoc />
    public partial class NewServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Posts",
                newName: "Url");

            migrationBuilder.AlterColumn<long>(
                name: "BlogId",
                table: "Posts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "PostId",
                table: "Posts",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateOnly>(
                name: "PostedDate",
                table: "Posts",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Posts",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BlogId",
                table: "Blogs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Blogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Blogs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostedDate",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Posts",
                newName: "Content");

            migrationBuilder.AlterColumn<int>(
                name: "BlogId",
                table: "Posts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                table: "Posts",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "BlogId",
                table: "Blogs",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
