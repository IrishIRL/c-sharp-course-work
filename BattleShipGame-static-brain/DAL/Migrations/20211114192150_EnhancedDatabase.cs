using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class EnhancedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipConfigs_GameConfigs_GameConfigid",
                table: "ShipConfigs");

            migrationBuilder.DropIndex(
                name: "IX_ShipConfigs_GameConfigid",
                table: "ShipConfigs");

            migrationBuilder.DropColumn(
                name: "GameConfigid",
                table: "ShipConfigs");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "GameConfigs",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "GameConfigId",
                table: "SavedGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GameSaveShipConfig",
                columns: table => new
                {
                    GameSaveSavedGameId = table.Column<int>(type: "int", nullable: false),
                    ShipConfigShipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSaveShipConfig", x => new { x.GameSaveSavedGameId, x.ShipConfigShipId });
                    table.ForeignKey(
                        name: "FK_GameSaveShipConfig_SavedGames_GameSaveSavedGameId",
                        column: x => x.GameSaveSavedGameId,
                        principalTable: "SavedGames",
                        principalColumn: "SavedGameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameSaveShipConfig_ShipConfigs_ShipConfigShipId",
                        column: x => x.ShipConfigShipId,
                        principalTable: "ShipConfigs",
                        principalColumn: "ShipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedGames_GameConfigId",
                table: "SavedGames",
                column: "GameConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSaveShipConfig_ShipConfigShipId",
                table: "GameSaveShipConfig",
                column: "ShipConfigShipId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedGames_GameConfigs_GameConfigId",
                table: "SavedGames",
                column: "GameConfigId",
                principalTable: "GameConfigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedGames_GameConfigs_GameConfigId",
                table: "SavedGames");

            migrationBuilder.DropTable(
                name: "GameSaveShipConfig");

            migrationBuilder.DropIndex(
                name: "IX_SavedGames_GameConfigId",
                table: "SavedGames");

            migrationBuilder.DropColumn(
                name: "GameConfigId",
                table: "SavedGames");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GameConfigs",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "GameConfigid",
                table: "ShipConfigs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShipConfigs_GameConfigid",
                table: "ShipConfigs",
                column: "GameConfigid");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipConfigs_GameConfigs_GameConfigid",
                table: "ShipConfigs",
                column: "GameConfigid",
                principalTable: "GameConfigs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
