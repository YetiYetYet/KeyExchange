using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class gamedemandNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_game_demands_games_game_id",
                schema: "keys_exchange",
                table: "game_demands");

            migrationBuilder.DropIndex(
                name: "ix_game_demands_game_id",
                schema: "keys_exchange",
                table: "game_demands");

            migrationBuilder.AddForeignKey(
                name: "fk_game_demands_games_game_id",
                schema: "keys_exchange",
                table: "game_demands",
                column: "user_id",
                principalSchema: "keys_exchange",
                principalTable: "games",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_game_demands_games_game_id",
                schema: "keys_exchange",
                table: "game_demands");

            migrationBuilder.CreateIndex(
                name: "ix_game_demands_game_id",
                schema: "keys_exchange",
                table: "game_demands",
                column: "game_id");

            migrationBuilder.AddForeignKey(
                name: "fk_game_demands_games_game_id",
                schema: "keys_exchange",
                table: "game_demands",
                column: "game_id",
                principalSchema: "keys_exchange",
                principalTable: "games",
                principalColumn: "id");
        }
    }
}
