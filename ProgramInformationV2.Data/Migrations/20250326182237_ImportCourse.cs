using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramInformationV2.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImportCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseImportEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateImported = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IncludeSections = table.Column<bool>(type: "bit", nullable: false),
                    IncludeTitleAndDescriptionOnly = table.Column<bool>(type: "bit", nullable: false),
                    Log = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rubric = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    UrlPattern = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseImportEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseImportEntries_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CourseImportEntries_SourceId",
                table: "CourseImportEntries",
                column: "SourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseImportEntries");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 2, 28, 16, 25, 28, 290, DateTimeKind.Local).AddTicks(8199));

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2025, 2, 28, 16, 25, 28, 290, DateTimeKind.Local).AddTicks(8087));
        }
    }
}
