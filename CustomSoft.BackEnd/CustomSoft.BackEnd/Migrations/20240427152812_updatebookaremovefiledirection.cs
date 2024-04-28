using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomSoft.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class updatebookaremovefiledirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileDirection",
                table: "Book");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileDirection",
                table: "Book",
                type: "text",
                nullable: true);
        }
    }
}
