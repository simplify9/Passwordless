using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SW.Auth.Web.Migrations
{
    public partial class Genesis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "main");

            migrationBuilder.CreateTable(
                name: "accounts",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", maxLength: 32, nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    account_info = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: true),
                    last_login = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    modified_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modified_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "authentication_tokens",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    password = table.Column<string>(type: "text", nullable: true),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authentication_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_authentication_tokens_account_account_id",
                        column: x => x.account_id,
                        principalSchema: "main",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_accounts_account_id",
                        column: x => x.account_id,
                        principalSchema: "main",
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_accounts_created_on",
                schema: "main",
                table: "accounts",
                column: "created_on");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_phone",
                schema: "main",
                table: "accounts",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_authentication_tokens_account_id",
                schema: "main",
                table: "authentication_tokens",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_authentication_tokens_created_on",
                schema: "main",
                table: "authentication_tokens",
                column: "created_on");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_account_id",
                schema: "main",
                table: "refresh_tokens",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_created_on",
                schema: "main",
                table: "refresh_tokens",
                column: "created_on");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "authentication_tokens",
                schema: "main");

            migrationBuilder.DropTable(
                name: "refresh_tokens",
                schema: "main");

            migrationBuilder.DropTable(
                name: "accounts",
                schema: "main");
        }
    }
}
