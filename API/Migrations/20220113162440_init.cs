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
                name: "keys_exchange");

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "keys_exchange",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    valid_upto = table.Column<DateTime>(type: "datetime2", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleted_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    soft_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    last_modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "application_users",
                schema: "keys_exchange",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    discord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    is_public = table.Column<bool>(type: "bit", nullable: false),
                    show_phone_number = table.Column<bool>(type: "bit", nullable: false),
                    show_email = table.Column<bool>(type: "bit", nullable: false),
                    show_discord = table.Column<bool>(type: "bit", nullable: false),
                    show_first_name = table.Column<bool>(type: "bit", nullable: false),
                    show_last_name = table.Column<bool>(type: "bit", nullable: false),
                    picture_uri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    last_login = table.Column<DateTime>(type: "datetime2", nullable: true),
                    access_failed_count = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    last_modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    soft_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    deleted_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_application_users_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "keys_exchange",
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "games",
                schema: "keys_exchange",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    last_modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    is_available = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    platforme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    generated_info = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "0"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reviews = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tumbnail_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    obtenaid_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    public_comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    owner_comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    admin_comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    received_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    given_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    given_to = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    soft_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    deleted_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                    table.ForeignKey(
                        name: "fk_games_application_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "keys_exchange",
                        principalTable: "application_users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "game_demands",
                schema: "keys_exchange",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    contact_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contact_info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    approuved = table.Column<bool>(type: "bit", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    soft_deleted = table.Column<bool>(type: "bit", nullable: false),
                    deleted_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_demands", x => x.id);
                    table.ForeignKey(
                        name: "fk_game_demands_application_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "keys_exchange",
                        principalTable: "application_users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_game_demands_games_game_id",
                        column: x => x.game_id,
                        principalSchema: "keys_exchange",
                        principalTable: "games",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_application_users_role_id",
                schema: "keys_exchange",
                table: "application_users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_users_username",
                schema: "keys_exchange",
                table: "application_users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_demands_game_id",
                schema: "keys_exchange",
                table: "game_demands",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_game_demands_user_id",
                schema: "keys_exchange",
                table: "game_demands",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_games_user_id",
                schema: "keys_exchange",
                table: "games",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_demands",
                schema: "keys_exchange");

            migrationBuilder.DropTable(
                name: "games",
                schema: "keys_exchange");

            migrationBuilder.DropTable(
                name: "application_users",
                schema: "keys_exchange");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "keys_exchange");
        }
    }
}
