using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCollaborationPlatform.Data.Migrations
{
    public partial class deletedFieldFromUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Boards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Boards");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleID",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
