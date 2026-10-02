using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramInformationV2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeminarFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SplitSeminarClasses",
                table: "Sources",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SplitSeminarClasses",
                table: "Sources");
        }
    }
}
