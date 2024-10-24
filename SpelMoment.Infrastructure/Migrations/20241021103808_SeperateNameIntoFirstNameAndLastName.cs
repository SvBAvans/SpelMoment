using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelMoment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeperateNameIntoFirstNameAndLastName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "SpelMoment",
                table: "Users",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "SpelMoment",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "SpelMoment",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "SpelMoment",
                table: "Users",
                newName: "Name");
        }
    }
}
