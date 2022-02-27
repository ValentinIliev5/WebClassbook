using Microsoft.EntityFrameworkCore.Migrations;

namespace WebClassbook.Migrations
{
    public partial class _231 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Absences_StudentID",
                table: "Absences",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_Students_StudentID",
                table: "Absences",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absences_Students_StudentID",
                table: "Absences");

            migrationBuilder.DropIndex(
                name: "IX_Absences_StudentID",
                table: "Absences");
        }
    }
}
