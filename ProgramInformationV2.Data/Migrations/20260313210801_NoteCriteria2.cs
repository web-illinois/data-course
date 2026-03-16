using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramInformationV2.Data.Migrations
{
    /// <inheritdoc />
    public partial class NoteCriteria2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "NoteTemplates",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "NoteTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkText",
                table: "NoteTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkUrl",
                table: "NoteTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 13, 16, 8, 1, 491, DateTimeKind.Local).AddTicks(2093));

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 13, 16, 8, 1, 491, DateTimeKind.Local).AddTicks(1960));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "NoteTemplates");

            migrationBuilder.DropColumn(
                name: "LinkText",
                table: "NoteTemplates");

            migrationBuilder.DropColumn(
                name: "LinkUrl",
                table: "NoteTemplates");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "NoteTemplates",
                newName: "Name");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 13, 15, 55, 52, 98, DateTimeKind.Local).AddTicks(4714));

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 13, 15, 55, 52, 98, DateTimeKind.Local).AddTicks(3408));
        }
    }
}
