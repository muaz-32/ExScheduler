using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExScheduler_Server.Migrations
{
    public partial class ChangedLinkExamDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LinkExamDates",
                table: "LinkExamDates");

            migrationBuilder.AddColumn<Guid>(
                name: "ProgrammeSemesterID",
                table: "LinkExamDates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinkExamDates",
                table: "LinkExamDates",
                columns: new[] { "LinkID", "ExamScheduleID", "ProgrammeSemesterID" });

            migrationBuilder.CreateIndex(
                name: "IX_LinkExamDates_ProgrammeSemesterID",
                table: "LinkExamDates",
                column: "ProgrammeSemesterID");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkExamDates_ProgramSemesters_ProgrammeSemesterID",
                table: "LinkExamDates",
                column: "ProgrammeSemesterID",
                principalTable: "ProgramSemesters",
                principalColumn: "programSemesterID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkExamDates_ProgramSemesters_ProgrammeSemesterID",
                table: "LinkExamDates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinkExamDates",
                table: "LinkExamDates");

            migrationBuilder.DropIndex(
                name: "IX_LinkExamDates_ProgrammeSemesterID",
                table: "LinkExamDates");

            migrationBuilder.DropColumn(
                name: "ProgrammeSemesterID",
                table: "LinkExamDates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinkExamDates",
                table: "LinkExamDates",
                columns: new[] { "LinkID", "ExamScheduleID" });
        }
    }
}
