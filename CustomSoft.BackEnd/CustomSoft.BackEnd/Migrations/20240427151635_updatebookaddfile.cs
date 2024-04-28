using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomSoft.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class updatebookaddfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "Book",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "Book");
        }
    }
}
