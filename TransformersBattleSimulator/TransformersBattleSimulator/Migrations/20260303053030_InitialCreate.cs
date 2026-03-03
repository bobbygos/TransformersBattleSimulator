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
                name: "BattleResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Winner = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Participants = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleResults", x => x.Id);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleResults");

            migrationBuilder.DropTable(
                name: "Transformers");
        }
    }
}
