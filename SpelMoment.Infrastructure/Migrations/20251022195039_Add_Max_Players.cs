using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelMoment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Max_Players : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxPlayers",
                schema: "SpelMoment",
                table: "Moments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPlayers",
                schema: "SpelMoment",
                table: "Moments");
        }
    }
}
