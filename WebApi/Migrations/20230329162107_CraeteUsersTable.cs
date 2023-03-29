using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class CraeteUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHashed = table.Column<string>(type: "text", nullable: false),
                    PublicKey = table.Column<string>(type: "text", nullable: false),
                    PrivateKeyEncrypted = table.Column<string>(type: "text", nullable: false),
                    MasterKeyEncrypted = table.Column<string>(type: "text", nullable: false),
                    ResetKeyEncrypted = table.Column<string>(type: "text", nullable: false),
                    HasherSaltEncrypted = table.Column<string>(type: "text", nullable: false),
                    HasherSaltHashed = table.Column<string>(type: "text", nullable: false),
                    OmCodeEncrypted = table.Column<string>(type: "text", nullable: false),
                    OmCodeHashed = table.Column<string>(type: "text", nullable: false),
                    RealName = table.Column<string>(type: "text", nullable: true),
                    Class = table.Column<string>(type: "text", nullable: true),
                    ImportAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_HasherSaltHashed",
                table: "Users",
                column: "HasherSaltHashed",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OmCodeHashed",
                table: "Users",
                column: "OmCodeHashed",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
