using Microsoft.EntityFrameworkCore.Migrations;

namespace TplDemo.Model.Migrations
{
    public partial class insertusermodule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserModule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MId = table.Column<string>(type: "varchar(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(36)", nullable: false),
                    Desc = table.Column<string>(type: "varchar(200)", nullable: true),
                    ParentId = table.Column<string>(type: "varchar(20)", nullable: true),
                    Url = table.Column<string>(type: "varchar(50)", nullable: true),
                    Icon = table.Column<string>(type: "varchar(30)", nullable: true),
                    Level = table.Column<string>(type: "char(1)", nullable: false),
                    Sequence = table.Column<string>(type: "varchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserModuleRel",
                columns: table => new
                {
                    UMID = table.Column<string>(type: "varchar(36)", nullable: false),
                    RId = table.Column<int>(nullable: false),
                    MId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "char(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModuleRel", x => x.UMID);
                    table.ForeignKey(
                        name: "FK_Module_Role_MId",
                        column: x => x.MId,
                        principalTable: "UserModule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Role_Module_RId",
                        column: x => x.RId,
                        principalTable: "UserRole",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserModuleRel_MId",
                table: "UserModuleRel",
                column: "MId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModuleRel_RId",
                table: "UserModuleRel",
                column: "RId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserModuleRel");

            migrationBuilder.DropTable(
                name: "UserModule");
        }
    }
}
