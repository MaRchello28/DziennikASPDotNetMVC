using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikASPDotNetMVC.Migrations
{
    /// <inheritdoc />
    public partial class changeMailModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "to",
                table: "Mails");

            migrationBuilder.AddColumn<int>(
                name: "toClassId",
                table: "Mails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuizAndSelectedClasseses_quizId",
                table: "QuizAndSelectedClasseses",
                column: "quizId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAndSelectedClasseses_Quizzes_quizId",
                table: "QuizAndSelectedClasseses",
                column: "quizId",
                principalTable: "Quizzes",
                principalColumn: "quizId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAndSelectedClasseses_Quizzes_quizId",
                table: "QuizAndSelectedClasseses");

            migrationBuilder.DropIndex(
                name: "IX_QuizAndSelectedClasseses_quizId",
                table: "QuizAndSelectedClasseses");

            migrationBuilder.DropColumn(
                name: "toClassId",
                table: "Mails");

            migrationBuilder.AddColumn<string>(
                name: "to",
                table: "Mails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
