using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebClassbook.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absence_Students_Absence",
                table: "Absence");

            migrationBuilder.DropForeignKey(
                name: "FK_Absence_Subject_SubjectID",
                table: "Absence");

            migrationBuilder.DropForeignKey(
                name: "FK_Absence_Teacher_TeacherID",
                table: "Absence");

            migrationBuilder.DropForeignKey(
                name: "FK_Admin_AspNetUsers_ApplicationUserID",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Admin_AdminID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teacher_TeacherID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Subject_SubjectID",
                table: "Exam");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Teacher_TeacherID",
                table: "Exam");

            migrationBuilder.DropForeignKey(
                name: "FK_Mark_Students_Mark",
                table: "Mark");

            migrationBuilder.DropForeignKey(
                name: "FK_Mark_Subject_SubjectID",
                table: "Mark");

            migrationBuilder.DropForeignKey(
                name: "FK_Mark_Teacher_TeacherID",
                table: "Mark");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacher_Teacher_TeachersId",
                table: "SubjectTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_AspNetUsers_ApplicationUserID",
                table: "Teacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mark",
                table: "Mark");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exam",
                table: "Exam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admin",
                table: "Admin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Absence",
                table: "Absence");

            migrationBuilder.RenameTable(
                name: "Teacher",
                newName: "Teachers");

            migrationBuilder.RenameTable(
                name: "Mark",
                newName: "Marks");

            migrationBuilder.RenameTable(
                name: "Exam",
                newName: "Exams");

            migrationBuilder.RenameTable(
                name: "Admin",
                newName: "Admins");

            migrationBuilder.RenameTable(
                name: "Absence",
                newName: "Absences");

            migrationBuilder.RenameIndex(
                name: "IX_Teacher_ApplicationUserID",
                table: "Teachers",
                newName: "IX_Teachers_ApplicationUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Mark_TeacherID",
                table: "Marks",
                newName: "IX_Marks_TeacherID");

            migrationBuilder.RenameIndex(
                name: "IX_Mark_SubjectID",
                table: "Marks",
                newName: "IX_Marks_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Mark_Mark",
                table: "Marks",
                newName: "IX_Marks_Mark");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_TeacherID",
                table: "Exams",
                newName: "IX_Exams_TeacherID");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_SubjectID",
                table: "Exams",
                newName: "IX_Exams_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Admin_ApplicationUserID",
                table: "Admins",
                newName: "IX_Admins_ApplicationUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Absence_TeacherID",
                table: "Absences",
                newName: "IX_Absences_TeacherID");

            migrationBuilder.RenameIndex(
                name: "IX_Absence_SubjectID",
                table: "Absences",
                newName: "IX_Absences_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Absence_Absence",
                table: "Absences",
                newName: "IX_Absences_Absence");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Marks",
                table: "Marks",
                column: "MarkID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exams",
                table: "Exams",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Absences",
                table: "Absences",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Remarks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPositive = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    TeacherID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remarks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Remarks_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Remarks_Teachers_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Remarks_StudentID",
                table: "Remarks",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Remarks_TeacherID",
                table: "Remarks",
                column: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_Students_Absence",
                table: "Absences",
                column: "Absence",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_Subject_SubjectID",
                table: "Absences",
                column: "SubjectID",
                principalTable: "Subject",
                principalColumn: "SubjectID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_Teachers_TeacherID",
                table: "Absences",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_AspNetUsers_ApplicationUserID",
                table: "Admins",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Admins_AdminID",
                table: "AspNetUsers",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teachers_TeacherID",
                table: "AspNetUsers",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Subject_SubjectID",
                table: "Exams",
                column: "SubjectID",
                principalTable: "Subject",
                principalColumn: "SubjectID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Teachers_TeacherID",
                table: "Exams",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Students_Mark",
                table: "Marks",
                column: "Mark",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Subject_SubjectID",
                table: "Marks",
                column: "SubjectID",
                principalTable: "Subject",
                principalColumn: "SubjectID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Teachers_TeacherID",
                table: "Marks",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacher_Teachers_TeachersId",
                table: "SubjectTeacher",
                column: "TeachersId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_AspNetUsers_ApplicationUserID",
                table: "Teachers",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absences_Students_Absence",
                table: "Absences");

            migrationBuilder.DropForeignKey(
                name: "FK_Absences_Subject_SubjectID",
                table: "Absences");

            migrationBuilder.DropForeignKey(
                name: "FK_Absences_Teachers_TeacherID",
                table: "Absences");

            migrationBuilder.DropForeignKey(
                name: "FK_Admins_AspNetUsers_ApplicationUserID",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Admins_AdminID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teachers_TeacherID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Subject_SubjectID",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Teachers_TeacherID",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Students_Mark",
                table: "Marks");

            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Subject_SubjectID",
                table: "Marks");

            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Teachers_TeacherID",
                table: "Marks");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacher_Teachers_TeachersId",
                table: "SubjectTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_AspNetUsers_ApplicationUserID",
                table: "Teachers");

            migrationBuilder.DropTable(
                name: "Remarks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Marks",
                table: "Marks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exams",
                table: "Exams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Absences",
                table: "Absences");

            migrationBuilder.RenameTable(
                name: "Teachers",
                newName: "Teacher");

            migrationBuilder.RenameTable(
                name: "Marks",
                newName: "Mark");

            migrationBuilder.RenameTable(
                name: "Exams",
                newName: "Exam");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "Admin");

            migrationBuilder.RenameTable(
                name: "Absences",
                newName: "Absence");

            migrationBuilder.RenameIndex(
                name: "IX_Teachers_ApplicationUserID",
                table: "Teacher",
                newName: "IX_Teacher_ApplicationUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_TeacherID",
                table: "Mark",
                newName: "IX_Mark_TeacherID");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_SubjectID",
                table: "Mark",
                newName: "IX_Mark_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_Mark",
                table: "Mark",
                newName: "IX_Mark_Mark");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_TeacherID",
                table: "Exam",
                newName: "IX_Exam_TeacherID");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_SubjectID",
                table: "Exam",
                newName: "IX_Exam_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Admins_ApplicationUserID",
                table: "Admin",
                newName: "IX_Admin_ApplicationUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Absences_TeacherID",
                table: "Absence",
                newName: "IX_Absence_TeacherID");

            migrationBuilder.RenameIndex(
                name: "IX_Absences_SubjectID",
                table: "Absence",
                newName: "IX_Absence_SubjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Absences_Absence",
                table: "Absence",
                newName: "IX_Absence_Absence");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mark",
                table: "Mark",
                column: "MarkID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exam",
                table: "Exam",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admin",
                table: "Admin",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Absence",
                table: "Absence",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_Students_Absence",
                table: "Absence",
                column: "Absence",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_Subject_SubjectID",
                table: "Absence",
                column: "SubjectID",
                principalTable: "Subject",
                principalColumn: "SubjectID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Absence_Teacher_TeacherID",
                table: "Absence",
                column: "TeacherID",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_AspNetUsers_ApplicationUserID",
                table: "Admin",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Admin_AdminID",
                table: "AspNetUsers",
                column: "AdminID",
                principalTable: "Admin",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teacher_TeacherID",
                table: "AspNetUsers",
                column: "TeacherID",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Subject_SubjectID",
                table: "Exam",
                column: "SubjectID",
                principalTable: "Subject",
                principalColumn: "SubjectID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Teacher_TeacherID",
                table: "Exam",
                column: "TeacherID",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mark_Students_Mark",
                table: "Mark",
                column: "Mark",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mark_Subject_SubjectID",
                table: "Mark",
                column: "SubjectID",
                principalTable: "Subject",
                principalColumn: "SubjectID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mark_Teacher_TeacherID",
                table: "Mark",
                column: "TeacherID",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacher_Teacher_TeachersId",
                table: "SubjectTeacher",
                column: "TeachersId",
                principalTable: "Teacher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_AspNetUsers_ApplicationUserID",
                table: "Teacher",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
