using System;
using System.Collections.Generic;
using Blueboard.Infrastructure.Persistence.Entities.Owned;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace Blueboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Create_StudentParties_And_StudentPartyCampaignPosts_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentParties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RichTextContent = table.Column<string>(type: "text", nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false)
                        .Annotation("Npgsql:TsVectorConfig", "hungarian")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Name", "Description", "RichTextContent" }),
                    Members = table.Column<List<StudentPartyMember>>(type: "jsonb", nullable: false),
                    ProgramPlanUrl = table.Column<string>(type: "text", nullable: false),
                    VideoUrl = table.Column<string>(type: "text", nullable: false),
                    PosterUrl = table.Column<string>(type: "text", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentParties_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentPartyCampaignPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RichTextContent = table.Column<string>(type: "text", nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false)
                        .Annotation("Npgsql:TsVectorConfig", "hungarian")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Name", "Description", "RichTextContent" }),
                    StudentPartyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPartyCampaignPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentPartyCampaignPosts_StudentParties_StudentPartyId",
                        column: x => x.StudentPartyId,
                        principalTable: "StudentParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentParties_SearchVector",
                table: "StudentParties",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_StudentParties_UserId",
                table: "StudentParties",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPartyCampaignPosts_SearchVector",
                table: "StudentPartyCampaignPosts",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPartyCampaignPosts_StudentPartyId",
                table: "StudentPartyCampaignPosts",
                column: "StudentPartyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentPartyCampaignPosts");

            migrationBuilder.DropTable(
                name: "StudentParties");
        }
    }
}
