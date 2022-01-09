using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class removeUserKeyTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refresh_token",
                schema: "key_exchange",
                table: "users");

            migrationBuilder.DropColumn(
                name: "refresh_token_expiration",
                schema: "key_exchange",
                table: "users");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                schema: "key_exchange",
                table: "claim_demands",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_claim_demands_user_id",
                schema: "key_exchange",
                table: "claim_demands",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_claim_demands_users_user_id",
                schema: "key_exchange",
                table: "claim_demands",
                column: "user_id",
                principalSchema: "key_exchange",
                principalTable: "users",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_claim_demands_users_user_id",
                schema: "key_exchange",
                table: "claim_demands");

            migrationBuilder.DropIndex(
                name: "ix_claim_demands_user_id",
                schema: "key_exchange",
                table: "claim_demands");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "key_exchange",
                table: "claim_demands");

            migrationBuilder.AddColumn<string>(
                name: "refresh_token",
                schema: "key_exchange",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "refresh_token_expiration",
                schema: "key_exchange",
                table: "users",
                type: "datetime2",
                nullable: true);
        }
    }
}
