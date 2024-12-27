using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikASPDotNetMVC.Migrations
{
    /// <inheritdoc />
    public partial class EditSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_User_replacementuserId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_replacementuserId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "replacementuserId",
                table: "Sessions");

            migrationBuilder.AddColumn<int>(
                name: "replacementTeacherId",
                table: "Lessons",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "replacementTeacherId",
                table: "Lessons");

            migrationBuilder.AddColumn<int>(
                name: "replacementuserId",
                table: "Sessions",
                type: "int",
                nullable: true);

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
        }
    }
}
