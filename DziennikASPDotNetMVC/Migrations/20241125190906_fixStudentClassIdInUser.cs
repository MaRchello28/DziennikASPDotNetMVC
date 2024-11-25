using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikASPDotNetMVC.Migrations
{
    /// <inheritdoc />
    public partial class fixStudentClassIdInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_StudentClasses_studentClassId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_studentClassId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "studentClassId",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "studentClassId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_studentClassId",
                table: "User",
                column: "studentClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_StudentClasses_studentClassId",
                table: "User",
                column: "studentClassId",
                principalTable: "StudentClasses",
                principalColumn: "studentClassId");
        }
    }
}
