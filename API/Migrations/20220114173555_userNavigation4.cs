using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class userNavigation4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_games_application_users_user_id",
                schema: "keys_exchange",
                table: "games");

            migrationBuilder.DropIndex(
                name: "ix_games_user_id",
                schema: "keys_exchange",
                table: "games");

            migrationBuilder.DropColumn(
                name: "user_id1",
                schema: "keys_exchange",
                table: "games");

            migrationBuilder.CreateIndex(
                name: "ix_games_user_id",
                schema: "keys_exchange",
                table: "games",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_games_application_users_user_id",
                schema: "keys_exchange",
                table: "games",
                column: "user_id",
                principalSchema: "keys_exchange",
                principalTable: "application_users",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_games_application_users_user_id",
                schema: "keys_exchange",
                table: "games");

            migrationBuilder.DropIndex(
                name: "ix_games_user_id",
                schema: "keys_exchange",
                table: "games");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id1",
                schema: "keys_exchange",
                table: "games",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_games_user_id",
                schema: "keys_exchange",
                table: "games",
                column: "user_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_games_application_users_user_id",
                schema: "keys_exchange",
                table: "games",
                column: "user_id1",
                principalSchema: "keys_exchange",
                principalTable: "application_users",
                principalColumn: "id");
        }
    }
}
