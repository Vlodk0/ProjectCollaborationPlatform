using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCollaborationPlatform.Data.Migrations
{
    public partial class addedNewColumnForProjectModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortInfo",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"UPDATE Projects
                                   SET ShortInfo = 'Make site'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortInfo",
                table: "Projects");
        }
    }
}
