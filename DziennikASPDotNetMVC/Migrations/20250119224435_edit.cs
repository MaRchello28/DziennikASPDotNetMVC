using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikASPDotNetMVC.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "toTeacherId",
                table: "Inquiryes");

            migrationBuilder.AddColumn<string>(
                name: "to",
                table: "Inquiryes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "to",
                table: "Inquiryes");

            migrationBuilder.AddColumn<int>(
                name: "toTeacherId",
                table: "Inquiryes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
