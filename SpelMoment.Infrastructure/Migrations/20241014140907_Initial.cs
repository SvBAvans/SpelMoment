using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpelMoment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SpelMoment");

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "SpelMoment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                schema: "SpelMoment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genre = table.Column<int>(type: "int", nullable: false),
                    Is18Plus = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    MomentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Moments",
                schema: "SpelMoment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OganiserId = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    maxPlayers = table.Column<int>(type: "int", nullable: false),
                    IsLactoseIntoleranceFriendly = table.Column<bool>(type: "bit", nullable: false),
                    IsVegetarianFriendly = table.Column<bool>(type: "bit", nullable: false),
                    IsNutAllergyFriendly = table.Column<bool>(type: "bit", nullable: false),
                    IsNonAlcoholicFriendly = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moments_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "SpelMoment",
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "SpelMoment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    HasLactoseIntolerance = table.Column<bool>(type: "bit", nullable: false),
                    HasNutAllergy = table.Column<bool>(type: "bit", nullable: false),
                    IsVegetarian = table.Column<bool>(type: "bit", nullable: false),
                    DrinksAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    MomentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Moments_MomentId",
                        column: x => x.MomentId,
                        principalSchema: "SpelMoment",
                        principalTable: "Moments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_MomentId",
                schema: "SpelMoment",
                table: "Games",
                column: "MomentId");

            migrationBuilder.CreateIndex(
                name: "IX_Moments_AddressId",
                schema: "SpelMoment",
                table: "Moments",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Moments_OganiserId",
                schema: "SpelMoment",
                table: "Moments",
                column: "OganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_MomentId",
                schema: "SpelMoment",
                table: "User",
                column: "MomentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Moments_MomentId",
                schema: "SpelMoment",
                table: "Games",
                column: "MomentId",
                principalSchema: "SpelMoment",
                principalTable: "Moments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Moments_User_OganiserId",
                schema: "SpelMoment",
                table: "Moments",
                column: "OganiserId",
                principalSchema: "SpelMoment",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Moments_MomentId",
                schema: "SpelMoment",
                table: "User");

            migrationBuilder.DropTable(
                name: "Games",
                schema: "SpelMoment");

            migrationBuilder.DropTable(
                name: "Moments",
                schema: "SpelMoment");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "SpelMoment");

            migrationBuilder.DropTable(
                name: "User",
                schema: "SpelMoment");
        }
    }
}
