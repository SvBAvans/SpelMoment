using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelMoment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStreet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Street",
                schema: "SpelMoment",
                table: "Moments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Street",
                schema: "SpelMoment",
                table: "Moments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
