using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "key_exchange");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_demands",
                schema: "key_exchange");

            migrationBuilder.DropTable(
                name: "role_claims",
                schema: "key_exchange");

            migrationBuilder.DropTable(
                name: "games",
                schema: "key_exchange");

            migrationBuilder.DropTable(
                name: "application_users",
                schema: "key_exchange");

            migrationBuilder.DropTable(
                name: "game_info_from_platforms",
                schema: "key_exchange");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "key_exchange");

            migrationBuilder.DropTable(
                name: "user_profiles",
                schema: "key_exchange");
        }
    }
}
