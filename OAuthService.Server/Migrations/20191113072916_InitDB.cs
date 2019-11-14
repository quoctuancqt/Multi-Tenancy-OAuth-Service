using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OAuthService.Server.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    ClientUri = table.Column<string>(nullable: true),
                    LogoUri = table.Column<string>(nullable: true),
                    ShortcutIconUri = table.Column<string>(nullable: true),
                    SubDomain = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Secret = table.Column<string>(nullable: true),
                    ClientPhone = table.Column<string>(nullable: true),
                    ClientEmail = table.Column<string>(nullable: true),
                    ClientAddress = table.Column<string>(nullable: true),
                    IsVerifyUser = table.Column<bool>(nullable: false),
                    ClientStatus = table.Column<int>(nullable: false),
                    IsSystem = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GlobalIdEntities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalIdEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    IsSystem = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientConfigurations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientConfigurations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserClients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientConfigurations_ClientId",
                table: "ClientConfigurations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClients_ClientId",
                table: "UserClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClients_UserId",
                table: "UserClients",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientConfigurations");

            migrationBuilder.DropTable(
                name: "GlobalIdEntities");

            migrationBuilder.DropTable(
                name: "UserClients");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
