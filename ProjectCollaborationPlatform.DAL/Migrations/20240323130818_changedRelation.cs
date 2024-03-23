using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCollaborationPlatform.Data.Migrations
{
    public partial class changedRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectDetails_ProjectDetailID",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectDetailID",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectDetailID",
                table: "Projects");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDetails_ProjectID",
                table: "ProjectDetails",
                column: "ProjectID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDetails_Projects_ProjectID",
                table: "ProjectDetails",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDetails_Projects_ProjectID",
                table: "ProjectDetails");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDetails_ProjectID",
                table: "ProjectDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectDetailID",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectDetailID",
                table: "Projects",
                column: "ProjectDetailID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectDetails_ProjectDetailID",
                table: "Projects",
                column: "ProjectDetailID",
                principalTable: "ProjectDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
