using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramInformationV2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Logs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldType",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 3, 28, 19, 27, 38, 2, DateTimeKind.Local).AddTicks(1663));

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 3, 28, 19, 27, 38, 2, DateTimeKind.Local).AddTicks(1550));

            migrationBuilder.CreateIndex(
                name: "IX_Logs_SourceId",
                table: "Logs",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Sources_SourceId",
                table: "Logs",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Sources_SourceId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_SourceId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "FieldType",
                table: "Logs");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 3, 26, 13, 22, 36, 842, DateTimeKind.Local).AddTicks(510));

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 3, 26, 13, 22, 36, 842, DateTimeKind.Local).AddTicks(418));
        }
    }
}
