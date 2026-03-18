using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramInformationV2.Data.Migrations
{
    /// <inheritdoc />
    public partial class NoteCriterial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionHtml",
                table: "NoteTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 18, 12, 41, 18, 631, DateTimeKind.Local).AddTicks(8050));

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 18, 12, 41, 18, 631, DateTimeKind.Local).AddTicks(7908));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionHtml",
                table: "NoteTemplates");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 14, 10, 35, 34, 747, DateTimeKind.Local).AddTicks(9823));

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2026, 3, 14, 10, 35, 34, 747, DateTimeKind.Local).AddTicks(9463));
        }
    }
}
