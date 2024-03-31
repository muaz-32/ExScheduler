﻿// <auto-generated />
using System;
using ExScheduler_Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExScheduler_Server.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ExScheduler_Server.Models.Admin", b =>
                {
                    b.Property<Guid>("AdminID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdminEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AdminName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AdminPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("AdminID");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.Course", b =>
                {
                    b.Property<Guid>("courseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("LinkslinkID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProgrammeSemesterprogramSemesterID")
                        .HasColumnType("uuid");

                    b.Property<string>("courseName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("examScheduleID")
                        .HasColumnType("uuid");

                    b.HasKey("courseID");

                    b.HasIndex("LinkslinkID");

                    b.HasIndex("ProgrammeSemesterprogramSemesterID");

                    b.HasIndex("examScheduleID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.Department", b =>
                {
                    b.Property<Guid>("departmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("departmentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("departmentID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.ExamSchedule", b =>
                {
                    b.Property<Guid>("examScheduleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("LinkslinkID")
                        .HasColumnType("uuid");

                    b.Property<string>("examDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("examScheduleID");

                    b.HasIndex("LinkslinkID");

                    b.ToTable("ExamSchedules");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.LinkCourse", b =>
                {
                    b.Property<Guid>("LinkID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseID")
                        .HasColumnType("uuid");

                    b.HasKey("LinkID", "CourseID");

                    b.HasIndex("CourseID");

                    b.ToTable("LinkCourses");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.LinkExamDate", b =>
                {
                    b.Property<Guid>("LinkID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ExamScheduleID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProgrammeSemesterID")
                        .HasColumnType("uuid");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.HasKey("LinkID", "ExamScheduleID", "ProgrammeSemesterID");

                    b.HasIndex("ExamScheduleID");

                    b.HasIndex("ProgrammeSemesterID");

                    b.ToTable("LinkExamDates");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.Links", b =>
                {
                    b.Property<Guid>("linkID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("linkname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("linkID");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.ProgrammeSemester", b =>
                {
                    b.Property<Guid>("programSemesterID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("departmentID")
                        .HasColumnType("uuid");

                    b.Property<string>("programSemesterName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("programSemesterID");

                    b.HasIndex("departmentID");

                    b.ToTable("ProgramSemesters");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.Students", b =>
                {
                    b.Property<int>("StudentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StudentID"));

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StudentEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StudentPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Validity")
                        .HasColumnType("boolean");

                    b.Property<Guid>("programSemesterID")
                        .HasColumnType("uuid");

                    b.HasKey("StudentID");

                    b.HasIndex("programSemesterID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.Course", b =>
                {
                    b.HasOne("ExScheduler_Server.Models.Links", null)
                        .WithMany("courses")
                        .HasForeignKey("LinkslinkID");

                    b.HasOne("ExScheduler_Server.Models.ProgrammeSemester", "ProgrammeSemester")
                        .WithMany("courses")
                        .HasForeignKey("ProgrammeSemesterprogramSemesterID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExScheduler_Server.Models.ExamSchedule", "ExamSchedule")
                        .WithMany()
                        .HasForeignKey("examScheduleID");

                    b.Navigation("ExamSchedule");

                    b.Navigation("ProgrammeSemester");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.ExamSchedule", b =>
                {
                    b.HasOne("ExScheduler_Server.Models.Links", null)
                        .WithMany("examSchedules")
                        .HasForeignKey("LinkslinkID");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.LinkCourse", b =>
                {
                    b.HasOne("ExScheduler_Server.Models.Course", "Course")
                        .WithMany("LinkCourses")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExScheduler_Server.Models.Links", "Link")
                        .WithMany("LinkCourses")
                        .HasForeignKey("LinkID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Link");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.LinkExamDate", b =>
                {
                    b.HasOne("ExScheduler_Server.Models.ExamSchedule", "ExamSchedule")
                        .WithMany("LinkExamDates")
                        .HasForeignKey("ExamScheduleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExScheduler_Server.Models.Links", "Link")
                        .WithMany("LinkExamDates")
                        .HasForeignKey("LinkID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExScheduler_Server.Models.ProgrammeSemester", "ProgrammeSemester")
                        .WithMany()
                        .HasForeignKey("ProgrammeSemesterID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExamSchedule");

                    b.Navigation("Link");

                    b.Navigation("ProgrammeSemester");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.ProgrammeSemester", b =>
                {
                    b.HasOne("ExScheduler_Server.Models.Department", "department")
                        .WithMany("ProgrammeSemesters")
                        .HasForeignKey("departmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("department");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.Students", b =>
                {
                    b.HasOne("ExScheduler_Server.Models.ProgrammeSemester", "program")
                        .WithMany("Students")
                        .HasForeignKey("programSemesterID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("program");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.Course", b =>
                {
                    b.Navigation("LinkCourses");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.Department", b =>
                {
                    b.Navigation("ProgrammeSemesters");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.ExamSchedule", b =>
                {
                    b.Navigation("LinkExamDates");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.Links", b =>
                {
                    b.Navigation("LinkCourses");

                    b.Navigation("LinkExamDates");

                    b.Navigation("courses");

                    b.Navigation("examSchedules");
                });

            modelBuilder.Entity("ExScheduler_Server.Models.ProgrammeSemester", b =>
                {
                    b.Navigation("Students");

                    b.Navigation("courses");
                });
#pragma warning restore 612, 618
        }
    }
}
