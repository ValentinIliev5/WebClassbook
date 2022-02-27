using Microsoft.EntityFrameworkCore.Migrations;

namespace WebClassbook.Migrations
{
    public partial class RemoveClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "Absences");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Absences",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
