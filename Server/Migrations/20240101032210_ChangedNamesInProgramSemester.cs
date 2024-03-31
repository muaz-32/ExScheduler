using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExScheduler_Server.Migrations
{
    public partial class ChangedNamesInProgramSemester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_ProgramSemesters_ProgrammeSemesterprogramID",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_ProgramSemesters_programID",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "programID",
                table: "Students",
                newName: "programSemesterID");

            migrationBuilder.RenameIndex(
                name: "IX_Students_programID",
                table: "Students",
                newName: "IX_Students_programSemesterID");

            migrationBuilder.RenameColumn(
                name: "programName",
                table: "ProgramSemesters",
                newName: "programSemesterName");

            migrationBuilder.RenameColumn(
                name: "programID",
                table: "ProgramSemesters",
                newName: "programSemesterID");

            migrationBuilder.RenameColumn(
                name: "ProgrammeSemesterprogramID",
                table: "Courses",
                newName: "ProgrammeSemesterprogramSemesterID");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_ProgrammeSemesterprogramID",
                table: "Courses",
                newName: "IX_Courses_ProgrammeSemesterprogramSemesterID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_ProgramSemesters_ProgrammeSemesterprogramSemesterID",
                table: "Courses",
                column: "ProgrammeSemesterprogramSemesterID",
                principalTable: "ProgramSemesters",
                principalColumn: "programSemesterID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_ProgramSemesters_programSemesterID",
                table: "Students",
                column: "programSemesterID",
                principalTable: "ProgramSemesters",
                principalColumn: "programSemesterID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_ProgramSemesters_ProgrammeSemesterprogramSemesterID",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_ProgramSemesters_programSemesterID",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "programSemesterID",
                table: "Students",
                newName: "programID");

            migrationBuilder.RenameIndex(
                name: "IX_Students_programSemesterID",
                table: "Students",
                newName: "IX_Students_programID");

            migrationBuilder.RenameColumn(
                name: "programSemesterName",
                table: "ProgramSemesters",
                newName: "programName");

            migrationBuilder.RenameColumn(
                name: "programSemesterID",
                table: "ProgramSemesters",
                newName: "programID");

            migrationBuilder.RenameColumn(
                name: "ProgrammeSemesterprogramSemesterID",
                table: "Courses",
                newName: "ProgrammeSemesterprogramID");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_ProgrammeSemesterprogramSemesterID",
                table: "Courses",
                newName: "IX_Courses_ProgrammeSemesterprogramID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_ProgramSemesters_ProgrammeSemesterprogramID",
                table: "Courses",
                column: "ProgrammeSemesterprogramID",
                principalTable: "ProgramSemesters",
                principalColumn: "programID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_ProgramSemesters_programID",
                table: "Students",
                column: "programID",
                principalTable: "ProgramSemesters",
                principalColumn: "programID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
