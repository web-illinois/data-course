using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramInformationV2.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecurityIsRestricted2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RestrictedIds",
                table: "SecurityEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "LastUpdated", "RestrictedIds" },
                values: new object[] { new DateTime(2025, 11, 18, 13, 56, 28, 906, DateTimeKind.Local).AddTicks(7058), "" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 11, 18, 13, 56, 28, 906, DateTimeKind.Local).AddTicks(6729));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestrictedIds",
                table: "SecurityEntries");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 11, 18, 12, 29, 24, 81, DateTimeKind.Local).AddTicks(3933));

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 11, 18, 12, 29, 24, 81, DateTimeKind.Local).AddTicks(3492));
        }
    }
}
