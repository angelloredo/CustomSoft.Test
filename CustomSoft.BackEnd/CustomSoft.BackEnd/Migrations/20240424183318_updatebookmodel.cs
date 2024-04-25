using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomSoft.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class updatebookmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Book",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Book",
                newName: "FileExtension");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Book",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FileExtension",
                table: "Book",
                newName: "LastName");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "Book",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
