using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramInformationV2.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixDynamicEF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: -1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "ApiSecretCurrent", "ApiSecretLastChanged", "ApiSecretPrevious", "BaseUrl", "Code", "CreatedByEmail", "IsActive", "IsTest", "LastUpdated", "RequestDeletion", "RequestDeletionByEmail", "Title", "UrlTemplate", "UseCourses", "UseCredentials", "UsePrograms", "UseRequirementSets", "UseSections" },
                values: new object[] { -1, "", null, "", "", "test", "jonker@illinois.edu", false, true, new DateTime(2026, 3, 18, 12, 41, 18, 631, DateTimeKind.Local).AddTicks(7908), false, "", "Test Entry", "", true, true, true, true, true });

            migrationBuilder.InsertData(
                table: "SecurityEntries",
                columns: new[] { "Id", "DepartmentTag", "Email", "IsActive", "IsFullAdmin", "IsOwner", "IsPublic", "IsRestricted", "LastUpdated", "RestrictedIds", "SourceId" },
                values: new object[] { -1, "", "jonker@illinois.edu", true, false, true, false, false, new DateTime(2026, 3, 18, 12, 41, 18, 631, DateTimeKind.Local).AddTicks(8050), "", -1 });
        }
    }
}
