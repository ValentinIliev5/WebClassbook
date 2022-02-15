using Microsoft.EntityFrameworkCore.Migrations;

namespace WebClassbook.Migrations
{
    public partial class Marktrsh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Marks_StudentID",
                table: "Marks",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Students_StudentID",
                table: "Marks",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Students_StudentID",
                table: "Marks");

            migrationBuilder.DropIndex(
                name: "IX_Marks_StudentID",
                table: "Marks");
        }
    }
}
