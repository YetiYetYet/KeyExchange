using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class adjustname2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_claim_demands_games_game_id",
                schema: "key_exchange",
                table: "claim_demands");

            migrationBuilder.DropForeignKey(
                name: "fk_claim_demands_users_user_id",
                schema: "key_exchange",
                table: "claim_demands");

            migrationBuilder.DropPrimaryKey(
                name: "pk_claim_demands",
                schema: "key_exchange",
                table: "claim_demands");

            migrationBuilder.RenameTable(
                name: "claim_demands",
                schema: "key_exchange",
                newName: "game_demands",
                newSchema: "key_exchange");

            migrationBuilder.RenameIndex(
                name: "ix_claim_demands_user_id",
                schema: "key_exchange",
                table: "game_demands",
                newName: "ix_game_demands_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_claim_demands_game_id",
                schema: "key_exchange",
                table: "game_demands",
                newName: "ix_game_demands_game_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_game_demands",
                schema: "key_exchange",
                table: "game_demands",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_game_demands_games_game_id",
                schema: "key_exchange",
                table: "game_demands",
                column: "game_id",
                principalSchema: "key_exchange",
                principalTable: "games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_game_demands_users_user_id",
                schema: "key_exchange",
                table: "game_demands",
                column: "user_id",
                principalSchema: "key_exchange",
                principalTable: "users",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_game_demands_games_game_id",
                schema: "key_exchange",
                table: "game_demands");

            migrationBuilder.DropForeignKey(
                name: "fk_game_demands_users_user_id",
                schema: "key_exchange",
                table: "game_demands");

            migrationBuilder.DropPrimaryKey(
                name: "pk_game_demands",
                schema: "key_exchange",
                table: "game_demands");

            migrationBuilder.RenameTable(
                name: "game_demands",
                schema: "key_exchange",
                newName: "claim_demands",
                newSchema: "key_exchange");

            migrationBuilder.RenameIndex(
                name: "ix_game_demands_user_id",
                schema: "key_exchange",
                table: "claim_demands",
                newName: "ix_claim_demands_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_game_demands_game_id",
                schema: "key_exchange",
                table: "claim_demands",
                newName: "ix_claim_demands_game_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_claim_demands",
                schema: "key_exchange",
                table: "claim_demands",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_claim_demands_games_game_id",
                schema: "key_exchange",
                table: "claim_demands",
                column: "game_id",
                principalSchema: "key_exchange",
                principalTable: "games",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_claim_demands_users_user_id",
                schema: "key_exchange",
                table: "claim_demands",
                column: "user_id",
                principalSchema: "key_exchange",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
