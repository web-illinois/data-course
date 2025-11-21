using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramInformationV2.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecurityAndAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiSecretCurrent",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApiSecretLastChanged",
                table: "Sources",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApiSecretPrevious",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Item",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 11, 19, 11, 33, 18, 926, DateTimeKind.Local).AddTicks(4809));

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "ApiSecretCurrent", "ApiSecretLastChanged", "ApiSecretPrevious", "LastUpdated" },
                values: new object[] { "", null, "", new DateTime(2025, 11, 19, 11, 33, 18, 926, DateTimeKind.Local).AddTicks(4520) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiSecretCurrent",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "ApiSecretLastChanged",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "ApiSecretPrevious",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "Item",
                table: "Logs");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 11, 18, 13, 56, 28, 906, DateTimeKind.Local).AddTicks(7058));

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 11, 18, 13, 56, 28, 906, DateTimeKind.Local).AddTicks(6729));
        }
    }
}
