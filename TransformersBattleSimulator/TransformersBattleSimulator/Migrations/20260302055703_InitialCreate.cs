using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransformersBattleSimulator.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transformers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NumberOfWins = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfLosses = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfDraws = table.Column<int>(type: "INTEGER", nullable: false),
                    TransformerType = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transformers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BattleResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WinnerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleResults_Transformers_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Transformers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BattleResultParticipants",
                columns: table => new
                {
                    ParticipantEntitiesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SimpleBattleResultId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleResultParticipants", x => new { x.ParticipantEntitiesId, x.SimpleBattleResultId });
                    table.ForeignKey(
                        name: "FK_BattleResultParticipants_BattleResults_SimpleBattleResultId",
                        column: x => x.SimpleBattleResultId,
                        principalTable: "BattleResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattleResultParticipants_Transformers_ParticipantEntitiesId",
                        column: x => x.ParticipantEntitiesId,
                        principalTable: "Transformers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattleResultParticipants_SimpleBattleResultId",
                table: "BattleResultParticipants",
                column: "SimpleBattleResultId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleResults_WinnerId",
                table: "BattleResults",
                column: "WinnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleResultParticipants");

            migrationBuilder.DropTable(
                name: "BattleResults");

            migrationBuilder.DropTable(
                name: "Transformers");
        }
    }
}
