using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class userNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_application_users_roles_role_id",
                schema: "keys_exchange",
                table: "application_users");

            migrationBuilder.AlterColumn<Guid>(
                name: "role_id",
                schema: "keys_exchange",
                table: "application_users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_application_users_roles_role_id",
                schema: "keys_exchange",
                table: "application_users",
                column: "role_id",
                principalSchema: "keys_exchange",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_application_users_roles_role_id",
                schema: "keys_exchange",
                table: "application_users");

            migrationBuilder.AlterColumn<Guid>(
                name: "role_id",
                schema: "keys_exchange",
                table: "application_users",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "fk_application_users_roles_role_id",
                schema: "keys_exchange",
                table: "application_users",
                column: "role_id",
                principalSchema: "keys_exchange",
                principalTable: "roles",
                principalColumn: "id");
        }
    }
}
