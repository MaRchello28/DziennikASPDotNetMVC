using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikASPDotNetMVC.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassSchedules",
                columns: table => new
                {
                    classScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSchedules", x => x.classScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    gradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<double>(type: "float", nullable: false),
                    wage = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentId = table.Column<int>(type: "int", nullable: false),
                    teacherId = table.Column<int>(type: "int", nullable: false),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sessionId = table.Column<int>(type: "int", nullable: false),
                    subjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.gradeId);
                });

            migrationBuilder.CreateTable(
                name: "StudentClasses",
                columns: table => new
                {
                    studentClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    number = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    letter = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    teacherId = table.Column<int>(type: "int", nullable: false),
                    classScheduleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClasses", x => x.studentClassId);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    subjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.subjectId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentClassId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userId);
                    table.ForeignKey(
                        name: "FK_User_StudentClasses_studentClassId",
                        column: x => x.studentClassId,
                        principalTable: "StudentClasses",
                        principalColumn: "studentClassId");
                });

            migrationBuilder.CreateTable(
                name: "Mails",
                columns: table => new
                {
                    mailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    read = table.Column<bool>(type: "bit", nullable: false),
                    send = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mails", x => x.mailId);
                    table.ForeignKey(
                        name: "FK_Mails_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    sessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subjectId = table.Column<int>(type: "int", nullable: false),
                    teacherId = table.Column<int>(type: "int", nullable: false),
                    dayOfTheWeek = table.Column<int>(type: "int", nullable: false),
                    hourFrom = table.Column<TimeSpan>(type: "time", nullable: false),
                    hourTo = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClassScheduleId = table.Column<int>(type: "int", nullable: false),
                    replacementuserId = table.Column<int>(type: "int", nullable: true),
                    sala = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.sessionId);
                    table.ForeignKey(
                        name: "FK_Sessions_ClassSchedules_ClassScheduleId",
                        column: x => x.ClassScheduleId,
                        principalTable: "ClassSchedules",
                        principalColumn: "classScheduleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sessions_User_replacementuserId",
                        column: x => x.replacementuserId,
                        principalTable: "User",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    lessonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nuberOfLesson = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.lessonId);
                    table.ForeignKey(
                        name: "FK_Lessons_Sessions_sessionId",
                        column: x => x.sessionId,
                        principalTable: "Sessions",
                        principalColumn: "sessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    attendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isPresent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentId = table.Column<int>(type: "int", nullable: false),
                    lessonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.attendanceId);
                    table.ForeignKey(
                        name: "FK_Attendances_Lessons_lessonId",
                        column: x => x.lessonId,
                        principalTable: "Lessons",
                        principalColumn: "lessonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_lessonId",
                table: "Attendances",
                column: "lessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_sessionId",
                table: "Lessons",
                column: "sessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Mails_userId",
                table: "Mails",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ClassScheduleId",
                table: "Sessions",
                column: "ClassScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_replacementuserId",
                table: "Sessions",
                column: "replacementuserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_studentClassId",
                table: "User",
                column: "studentClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Mails");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "ClassSchedules");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "StudentClasses");
        }
    }
}
