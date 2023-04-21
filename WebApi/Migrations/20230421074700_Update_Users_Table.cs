using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Update_Users_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasherSaltHashed",
                table: "Users",
                newName: "HasherSalt");

            migrationBuilder.RenameIndex(
                name: "IX_Users_HasherSaltHashed",
                table: "Users",
                newName: "IX_Users_HasherSalt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasherSalt",
                table: "Users",
                newName: "HasherSaltHashed");

            migrationBuilder.RenameIndex(
                name: "IX_Users_HasherSalt",
                table: "Users",
                newName: "IX_Users_HasherSaltHashed");
        }
    }
}
