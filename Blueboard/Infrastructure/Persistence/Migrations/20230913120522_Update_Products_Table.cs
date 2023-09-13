using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blueboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Products_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserLimit",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "UserLimited",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserLimit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserLimited",
                table: "Products");
        }
    }
}
