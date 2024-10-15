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
                name: "Users",
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
                    Address_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Moments",
                schema: "SpelMoment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganiserId = table.Column<int>(type: "int", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLactoseIntoleranceFriendly = table.Column<bool>(type: "bit", nullable: false),
                    IsVegetarianFriendly = table.Column<bool>(type: "bit", nullable: false),
                    IsNutAllergyFriendly = table.Column<bool>(type: "bit", nullable: false),
                    IsNonAlcoholicFriendly = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moments_Users_OrganiserId",
                        column: x => x.OrganiserId,
                        principalSchema: "SpelMoment",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                    table.ForeignKey(
                        name: "FK_Games_Moments_MomentId",
                        column: x => x.MomentId,
                        principalSchema: "SpelMoment",
                        principalTable: "Moments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MomentUser",
                schema: "SpelMoment",
                columns: table => new
                {
                    PlayersId = table.Column<int>(type: "int", nullable: false),
                    RegisteredMomentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MomentUser", x => new { x.PlayersId, x.RegisteredMomentsId });
                    table.ForeignKey(
                        name: "FK_MomentUser_Moments_RegisteredMomentsId",
                        column: x => x.RegisteredMomentsId,
                        principalSchema: "SpelMoment",
                        principalTable: "Moments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MomentUser_Users_PlayersId",
                        column: x => x.PlayersId,
                        principalSchema: "SpelMoment",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_MomentId",
                schema: "SpelMoment",
                table: "Games",
                column: "MomentId");

            migrationBuilder.CreateIndex(
                name: "IX_Moments_OrganiserId",
                schema: "SpelMoment",
                table: "Moments",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_MomentUser_RegisteredMomentsId",
                schema: "SpelMoment",
                table: "MomentUser",
                column: "RegisteredMomentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games",
                schema: "SpelMoment");

            migrationBuilder.DropTable(
                name: "MomentUser",
                schema: "SpelMoment");

            migrationBuilder.DropTable(
                name: "Moments",
                schema: "SpelMoment");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "SpelMoment");
        }
    }
}
