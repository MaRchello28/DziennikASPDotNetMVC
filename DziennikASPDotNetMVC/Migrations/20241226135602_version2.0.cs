using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikASPDotNetMVC.Migrations
{
    /// <inheritdoc />
    public partial class version20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_ClassSchedules_ClassScheduleId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "ClassSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_ClassScheduleId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "classScheduleId",
                table: "StudentClasses");

            migrationBuilder.DropColumn(
                name: "ClassScheduleId",
                table: "Sessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "classScheduleId",
                table: "StudentClasses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassScheduleId",
                table: "Sessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ClassScheduleId",
                table: "Sessions",
                column: "ClassScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_ClassSchedules_ClassScheduleId",
                table: "Sessions",
                column: "ClassScheduleId",
                principalTable: "ClassSchedules",
                principalColumn: "classScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
