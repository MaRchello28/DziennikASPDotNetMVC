using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikASPDotNetMVC.Migrations
{
    /// <inheritdoc />
    public partial class StuentWithClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "StudentWithClasses",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "studentClassId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_studentClassId",
                table: "User",
                column: "studentClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWithClasses_studentClassId",
                table: "StudentWithClasses",
                column: "studentClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWithClasses_studentId",
                table: "StudentWithClasses",
                column: "studentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentWithClasses_StudentClasses_studentClassId",
                table: "StudentWithClasses",
                column: "studentClassId",
                principalTable: "StudentClasses",
                principalColumn: "studentClassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentWithClasses_User_studentId",
                table: "StudentWithClasses",
                column: "studentId",
                principalTable: "User",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_StudentWithClasses_StudentClasses_studentClassId",
                table: "StudentWithClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentWithClasses_User_studentId",
                table: "StudentWithClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_User_StudentClasses_studentClassId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_studentClassId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_StudentWithClasses_studentClassId",
                table: "StudentWithClasses");

            migrationBuilder.DropIndex(
                name: "IX_StudentWithClasses_studentId",
                table: "StudentWithClasses");

            migrationBuilder.DropColumn(
                name: "studentClassId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StudentWithClasses",
                newName: "id");
        }
    }
}
