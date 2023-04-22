using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    KeyProtected = table.Column<string>(type: "text", nullable: false),
                    KeyHashed = table.Column<string>(type: "text", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportKeys", x => x.Id);
                });

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
                    IdSaltEncrypted = table.Column<string>(type: "text", nullable: false),
                    IdSaltHashed = table.Column<string>(type: "text", nullable: false),
                    HasherSalt = table.Column<string>(type: "text", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "GradeImports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    KeyEncrypeted = table.Column<string>(type: "text", nullable: false),
                    JsonEncrypted = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeImports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeImports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalAccessTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalAccessTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalAccessTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradeImports_UserId",
                table: "GradeImports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportKeys_KeyHashed",
                table: "ImportKeys",
                column: "KeyHashed",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalAccessTokens_Token",
                table: "PersonalAccessTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalAccessTokens_UserId",
                table: "PersonalAccessTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_HasherSalt",
                table: "Users",
                column: "HasherSalt",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdSaltHashed",
                table: "Users",
                column: "IdSaltHashed",
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
                name: "GradeImports");

            migrationBuilder.DropTable(
                name: "ImportKeys");

            migrationBuilder.DropTable(
                name: "PersonalAccessTokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
