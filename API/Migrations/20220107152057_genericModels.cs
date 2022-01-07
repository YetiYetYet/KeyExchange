using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class genericModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "key_exchange");

            migrationBuilder.CreateTable(
                name: "games",
                schema: "key_exchange",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    last_modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    is_available = table.Column<bool>(type: "bit", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    platforme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    obtenaid_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    public_comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    admin_comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    received_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    given_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    given_to = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleted_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "key_exchange",
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
                    last_modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "claim_demands",
                schema: "key_exchange",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contact_info = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    last_modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    deleted_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_claim_demands", x => x.id);
                    table.ForeignKey(
                        name: "fk_claim_demands_games_game_id",
                        column: x => x.game_id,
                        principalSchema: "key_exchange",
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "key_exchange",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    discord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    refresh_token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    refresh_token_expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    access_failed_count = table.Column<int>(type: "int", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    last_modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    deleted_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "key_exchange",
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                schema: "key_exchange",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    show_phone_number = table.Column<bool>(type: "bit", nullable: false),
                    show_email = table.Column<bool>(type: "bit", nullable: false),
                    show_discord = table.Column<bool>(type: "bit", nullable: false),
                    show_first_name = table.Column<bool>(type: "bit", nullable: false),
                    show_last_name = table.Column<bool>(type: "bit", nullable: false),
                    picture_uri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deleted_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    last_modified_by = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    last_modified_on = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_profiles_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "key_exchange",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_claim_demands_game_id",
                schema: "key_exchange",
                table: "claim_demands",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_profiles_user_id",
                schema: "key_exchange",
                table: "user_profiles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_role_id",
                schema: "key_exchange",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_username",
                schema: "key_exchange",
                table: "users",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "claim_demands",
                schema: "key_exchange");

            migrationBuilder.DropTable(
                name: "user_profiles",
                schema: "key_exchange");

            migrationBuilder.DropTable(
                name: "games",
                schema: "key_exchange");

            migrationBuilder.DropTable(
                name: "users",
                schema: "key_exchange");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "key_exchange");
        }
    }
}
