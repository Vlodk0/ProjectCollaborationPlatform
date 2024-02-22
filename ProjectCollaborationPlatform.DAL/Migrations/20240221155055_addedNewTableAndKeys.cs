using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCollaborationPlatform.Data.Migrations
{
    public partial class addedNewTableAndKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectOwnerID",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProjectOwner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectOwner", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectOwnerID",
                table: "Projects",
                column: "ProjectOwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectOwner_ProjectOwnerID",
                table: "Projects",
                column: "ProjectOwnerID",
                principalTable: "ProjectOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectOwner_ProjectOwnerID",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectOwner");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectOwnerID",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectOwnerID",
                table: "Projects");
        }
    }
}
