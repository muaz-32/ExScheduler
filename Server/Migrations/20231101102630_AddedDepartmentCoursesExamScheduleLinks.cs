using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExScheduler_Server.Migrations
{
    public partial class AddedDepartmentCoursesExamScheduleLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "programID",
                table: "Students",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    departmentID = table.Column<Guid>(type: "uuid", nullable: false),
                    departmentName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.departmentID);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    linkID = table.Column<Guid>(type: "uuid", nullable: false),
                    linkname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.linkID);
                });

            migrationBuilder.CreateTable(
                name: "ProgramSemesters",
                columns: table => new
                {
                    programID = table.Column<Guid>(type: "uuid", nullable: false),
                    programName = table.Column<string>(type: "text", nullable: false),
                    departmentID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramSemesters", x => x.programID);
                    table.ForeignKey(
                        name: "FK_ProgramSemesters_Departments_departmentID",
                        column: x => x.departmentID,
                        principalTable: "Departments",
                        principalColumn: "departmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamSchedules",
                columns: table => new
                {
                    examScheduleID = table.Column<Guid>(type: "uuid", nullable: false),
                    examDate = table.Column<string>(type: "text", nullable: false),
                    LinkslinkID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSchedules", x => x.examScheduleID);
                    table.ForeignKey(
                        name: "FK_ExamSchedules_Links_LinkslinkID",
                        column: x => x.LinkslinkID,
                        principalTable: "Links",
                        principalColumn: "linkID");
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    courseID = table.Column<Guid>(type: "uuid", nullable: false),
                    courseName = table.Column<string>(type: "text", nullable: false),
                    examScheduleID = table.Column<Guid>(type: "uuid", nullable: true),
                    ProgrammeSemesterprogramID = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkslinkID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.courseID);
                    table.ForeignKey(
                        name: "FK_Courses_ExamSchedules_examScheduleID",
                        column: x => x.examScheduleID,
                        principalTable: "ExamSchedules",
                        principalColumn: "examScheduleID");
                    table.ForeignKey(
                        name: "FK_Courses_Links_LinkslinkID",
                        column: x => x.LinkslinkID,
                        principalTable: "Links",
                        principalColumn: "linkID");
                    table.ForeignKey(
                        name: "FK_Courses_ProgramSemesters_ProgrammeSemesterprogramID",
                        column: x => x.ProgrammeSemesterprogramID,
                        principalTable: "ProgramSemesters",
                        principalColumn: "programID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkExamDates",
                columns: table => new
                {
                    LinkID = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamScheduleID = table.Column<Guid>(type: "uuid", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkExamDates", x => new { x.LinkID, x.ExamScheduleID });
                    table.ForeignKey(
                        name: "FK_LinkExamDates_ExamSchedules_ExamScheduleID",
                        column: x => x.ExamScheduleID,
                        principalTable: "ExamSchedules",
                        principalColumn: "examScheduleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkExamDates_Links_LinkID",
                        column: x => x.LinkID,
                        principalTable: "Links",
                        principalColumn: "linkID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkCourses",
                columns: table => new
                {
                    LinkID = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkCourses", x => new { x.LinkID, x.CourseID });
                    table.ForeignKey(
                        name: "FK_LinkCourses_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "courseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkCourses_Links_LinkID",
                        column: x => x.LinkID,
                        principalTable: "Links",
                        principalColumn: "linkID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_programID",
                table: "Students",
                column: "programID");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_examScheduleID",
                table: "Courses",
                column: "examScheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LinkslinkID",
                table: "Courses",
                column: "LinkslinkID");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ProgrammeSemesterprogramID",
                table: "Courses",
                column: "ProgrammeSemesterprogramID");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedules_LinkslinkID",
                table: "ExamSchedules",
                column: "LinkslinkID");

            migrationBuilder.CreateIndex(
                name: "IX_LinkCourses_CourseID",
                table: "LinkCourses",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_LinkExamDates_ExamScheduleID",
                table: "LinkExamDates",
                column: "ExamScheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramSemesters_departmentID",
                table: "ProgramSemesters",
                column: "departmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_ProgramSemesters_programID",
                table: "Students",
                column: "programID",
                principalTable: "ProgramSemesters",
                principalColumn: "programID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_ProgramSemesters_programID",
                table: "Students");

            migrationBuilder.DropTable(
                name: "LinkCourses");

            migrationBuilder.DropTable(
                name: "LinkExamDates");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "ExamSchedules");

            migrationBuilder.DropTable(
                name: "ProgramSemesters");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Students_programID",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "programID",
                table: "Students");
        }
    }
}
