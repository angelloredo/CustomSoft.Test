using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomSoft.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class updateauthorid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookAuthor",
                table: "Book");

            migrationBuilder.AlterColumn<string>(
                name: "BookAuthorGuid",
                table: "BookAuthor",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "Book",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookAuthorGuid",
                table: "Book",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Book",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Book",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AcademicGradeGuid",
                table: "AcademicGrade",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "BookAuthorGuid",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Book");

            migrationBuilder.AlterColumn<string>(
                name: "BookAuthorGuid",
                table: "BookAuthor",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "BookAuthor",
                table: "Book",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AcademicGradeGuid",
                table: "AcademicGrade",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
