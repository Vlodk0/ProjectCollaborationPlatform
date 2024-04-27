using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCollaborationPlatform.Data.Migrations
{
    public partial class deletedUserModelAndChangedRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoFiles_Users_UserId",
                table: "PhotoFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectOwner_ProjectOwnerID",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectOwner",
                table: "ProjectOwner");

            migrationBuilder.RenameTable(
                name: "ProjectOwner",
                newName: "ProjectOwners");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PhotoFiles",
                newName: "ProjectOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PhotoFiles_UserId",
                table: "PhotoFiles",
                newName: "IX_PhotoFiles_ProjectOwnerId");

            migrationBuilder.AddColumn<Guid>(
                name: "DeveloperId",
                table: "PhotoFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Developers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Developers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Developers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Developers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ProjectOwners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ProjectOwners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectOwners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ProjectOwners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectOwners",
                table: "ProjectOwners",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoFiles_DeveloperId",
                table: "PhotoFiles",
                column: "DeveloperId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectOwners_ProjectOwnerID",
                table: "Projects",
                column: "ProjectOwnerID",
                principalTable: "ProjectOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoFiles_Developers_DeveloperId",
                table: "PhotoFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_PhotoFiles_ProjectOwners_ProjectOwnerId",
                table: "PhotoFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectOwners_ProjectOwnerID",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_PhotoFiles_DeveloperId",
                table: "PhotoFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectOwners",
                table: "ProjectOwners");

            migrationBuilder.DropColumn(
                name: "DeveloperId",
                table: "PhotoFiles");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ProjectOwners");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ProjectOwners");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProjectOwners");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ProjectOwners");

            migrationBuilder.RenameTable(
                name: "ProjectOwners",
                newName: "ProjectOwner");

            migrationBuilder.RenameColumn(
                name: "ProjectOwnerId",
                table: "PhotoFiles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PhotoFiles_ProjectOwnerId",
                table: "PhotoFiles",
                newName: "IX_PhotoFiles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectOwner",
                table: "ProjectOwner",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoFiles_Users_UserId",
                table: "PhotoFiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectOwner_ProjectOwnerID",
                table: "Projects",
                column: "ProjectOwnerID",
                principalTable: "ProjectOwner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
