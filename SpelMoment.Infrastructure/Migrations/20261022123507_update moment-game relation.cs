using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelMoment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatemomentgamerelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Moments_MomentId",
                schema: "SpelMoment",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "MomentId",
                schema: "SpelMoment",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Moments_MomentId",
                schema: "SpelMoment",
                table: "Games",
                column: "MomentId",
                principalSchema: "SpelMoment",
                principalTable: "Moments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Moments_MomentId",
                schema: "SpelMoment",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "MomentId",
                schema: "SpelMoment",
                table: "Games",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Moments_MomentId",
                schema: "SpelMoment",
                table: "Games",
                column: "MomentId",
                principalSchema: "SpelMoment",
                principalTable: "Moments",
                principalColumn: "Id");
        }
    }
}
