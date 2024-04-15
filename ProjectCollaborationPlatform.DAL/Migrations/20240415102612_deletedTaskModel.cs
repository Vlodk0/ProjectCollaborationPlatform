using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCollaborationPlatform.Data.Migrations
{
    public partial class deletedTaskModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FunctionalityBlocks",
                newName: "Task");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Task",
                table: "FunctionalityBlocks",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FunctionalityBlockID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_FunctionalityBlocks_FunctionalityBlockID",
                        column: x => x.FunctionalityBlockID,
                        principalTable: "FunctionalityBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_FunctionalityBlockID",
                table: "Tasks",
                column: "FunctionalityBlockID");
        }
    }
}
