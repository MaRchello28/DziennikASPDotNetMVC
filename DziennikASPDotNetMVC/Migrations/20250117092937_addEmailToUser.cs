using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DziennikASPDotNetMVC.Migrations
{
    /// <inheritdoc />
    public partial class addEmailToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "User");
        }
    }
}
