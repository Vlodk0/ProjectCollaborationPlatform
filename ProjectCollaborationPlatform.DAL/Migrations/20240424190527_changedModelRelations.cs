using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCollaborationPlatform.Data.Migrations
{
    public partial class changedModelRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoFiles_Developers_DeveloperId",
                table: "PhotoFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotoFiles_ProjectOwners_ProjectOwnerId",
                table: "PhotoFiles");

            migrationBuilder.DropIndex(
                name: "IX_PhotoFiles_DeveloperId",
                table: "PhotoFiles");

            migrationBuilder.DropIndex(
                name: "IX_PhotoFiles_ProjectOwnerId",
                table: "PhotoFiles");

            migrationBuilder.DropColumn(
                name: "DeveloperId",
                table: "PhotoFiles");

            migrationBuilder.DropColumn(
                name: "ProjectOwnerId",
                table: "PhotoFiles");

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoFileId",
                table: "ProjectOwners",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoFileId",
                table: "Developers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectOwners_PhotoFileId",
                table: "ProjectOwners",
                column: "PhotoFileId",
                unique: true,
                filter: "[PhotoFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Developers_PhotoFileId",
                table: "Developers",
                column: "PhotoFileId",
                unique: true,
                filter: "[PhotoFileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Developers_PhotoFiles_PhotoFileId",
                table: "Developers",
                column: "PhotoFileId",
                principalTable: "PhotoFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectOwners_PhotoFiles_PhotoFileId",
                table: "ProjectOwners",
                column: "PhotoFileId",
                principalTable: "PhotoFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Developers_PhotoFiles_PhotoFileId",
                table: "Developers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectOwners_PhotoFiles_PhotoFileId",
                table: "ProjectOwners");

            migrationBuilder.DropIndex(
                name: "IX_ProjectOwners_PhotoFileId",
                table: "ProjectOwners");

            migrationBuilder.DropIndex(
                name: "IX_Developers_PhotoFileId",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "PhotoFileId",
                table: "ProjectOwners");

            migrationBuilder.DropColumn(
                name: "PhotoFileId",
                table: "Developers");

            migrationBuilder.AddColumn<Guid>(
                name: "DeveloperId",
                table: "PhotoFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectOwnerId",
                table: "PhotoFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PhotoFiles_DeveloperId",
                table: "PhotoFiles",
                column: "DeveloperId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhotoFiles_ProjectOwnerId",
                table: "PhotoFiles",
                column: "ProjectOwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoFiles_Developers_DeveloperId",
                table: "PhotoFiles",
                column: "DeveloperId",
                principalTable: "Developers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoFiles_ProjectOwners_ProjectOwnerId",
                table: "PhotoFiles",
                column: "ProjectOwnerId",
                principalTable: "ProjectOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
