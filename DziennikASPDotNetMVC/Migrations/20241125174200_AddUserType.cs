using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikASPDotNetMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddUserType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_studentId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Teachers_teacherId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_teacherId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Grades_studentId",
                table: "Grades");

            migrationBuilder.AddColumn<int>(
                name: "studentClassId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "replacementuserId",
                table: "Sessions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_studentClassId",
                table: "User",
                column: "studentClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_replacementuserId",
                table: "Sessions",
                column: "replacementuserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_User_replacementuserId",
                table: "Sessions",
                column: "replacementuserId",
                principalTable: "User",
                principalColumn: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_StudentClasses_studentClassId",
                table: "User",
                column: "studentClassId",
                principalTable: "StudentClasses",
                principalColumn: "studentClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_User_replacementuserId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_User_StudentClasses_studentClassId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_studentClassId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_replacementuserId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "studentClassId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "type",
                table: "User");

            migrationBuilder.DropColumn(
                name: "replacementuserId",
                table: "Sessions");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    adminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.adminId);
                    table.ForeignKey(
                        name: "FK_Admins_User_adminId",
                        column: x => x.adminId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    parentId = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.parentId);
                    table.ForeignKey(
                        name: "FK_Parents_User_parentId",
                        column: x => x.parentId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    teacherId = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.teacherId);
                    table.ForeignKey(
                        name: "FK_Teachers_User_teacherId",
                        column: x => x.teacherId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    studentId = table.Column<int>(type: "int", nullable: false),
                    parentId = table.Column<int>(type: "int", nullable: true),
                    studentClassId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.studentId);
                    table.ForeignKey(
                        name: "FK_Students_Parents_parentId",
                        column: x => x.parentId,
                        principalTable: "Parents",
                        principalColumn: "parentId");
                    table.ForeignKey(
                        name: "FK_Students_StudentClasses_studentClassId",
                        column: x => x.studentClassId,
                        principalTable: "StudentClasses",
                        principalColumn: "studentClassId");
                    table.ForeignKey(
                        name: "FK_Students_User_studentId",
                        column: x => x.studentId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_teacherId",
                table: "Sessions",
                column: "teacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_studentId",
                table: "Grades",
                column: "studentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_parentId",
                table: "Students",
                column: "parentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_studentClassId",
                table: "Students",
                column: "studentClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_studentId",
                table: "Grades",
                column: "studentId",
                principalTable: "Students",
                principalColumn: "studentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Teachers_teacherId",
                table: "Sessions",
                column: "teacherId",
                principalTable: "Teachers",
                principalColumn: "teacherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
