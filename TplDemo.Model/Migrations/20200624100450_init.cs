using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TplDemo.Model.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(36)", nullable: false),
                    Password = table.Column<string>(type: "varchar(36)", nullable: false),
                    Email = table.Column<string>(type: "varchar(36)", nullable: false),
                    Sex = table.Column<bool>(type: "bit", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Birth = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", nullable: true),
                    Address = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(36)", nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleRel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleRel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_UserId",
                        column: x => x.RoleId,
                        principalTable: "UserRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserId_Role",
                        column: x => x.UserId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleRel_RoleId",
                table: "UserRoleRel",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleRel_UserId",
                table: "UserRoleRel",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoleRel");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
