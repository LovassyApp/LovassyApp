using System;
using System.Collections.Generic;
using Blueboard.Infrastructure.Persistence.Entities.Owned;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Blueboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Create_ImageVoting_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:grade_type", "regular_grade,behaviour_grade,diligence_grade")
                .Annotation("Npgsql:Enum:image_voting_type", "single_choice,rating")
                .Annotation("Npgsql:Enum:lolo_type", "from_grades,from_request")
                .OldAnnotation("Npgsql:Enum:grade_type", "regular_grade,behaviour_grade,diligence_grade")
                .OldAnnotation("Npgsql:Enum:lolo_type", "from_grades,from_request");

            migrationBuilder.CreateTable(
                name: "ImageVotings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Aspects = table.Column<List<ImageVotingAspect>>(type: "jsonb", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    ShowUploaderInfo = table.Column<bool>(type: "boolean", nullable: false),
                    UploaderUserGroupId = table.Column<int>(type: "integer", nullable: false),
                    MaxUploadsPerUser = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageVotings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageVotings_UserGroups_UploaderUserGroupId",
                        column: x => x.UploaderUserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImageVotingEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageVotingId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageVotingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageVotingEntries_ImageVotings_ImageVotingId",
                        column: x => x.ImageVotingId,
                        principalTable: "ImageVotings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageVotingEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageVotingChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageVotingId = table.Column<int>(type: "integer", nullable: false),
                    ImageVotingEntryId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageVotingChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageVotingChoices_ImageVotingEntries_ImageVotingEntryId",
                        column: x => x.ImageVotingEntryId,
                        principalTable: "ImageVotingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageVotingChoices_ImageVotings_ImageVotingId",
                        column: x => x.ImageVotingId,
                        principalTable: "ImageVotings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageVotingChoices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageVotingEntryRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AspectKey = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    ImageVotingEntryId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageVotingEntryRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageVotingEntryRatings_ImageVotingEntries_ImageVotingEntry~",
                        column: x => x.ImageVotingEntryId,
                        principalTable: "ImageVotingEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageVotingEntryRatings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageVotingChoices_ImageVotingEntryId",
                table: "ImageVotingChoices",
                column: "ImageVotingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageVotingChoices_ImageVotingId",
                table: "ImageVotingChoices",
                column: "ImageVotingId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageVotingChoices_UserId",
                table: "ImageVotingChoices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageVotingEntries_ImageVotingId",
                table: "ImageVotingEntries",
                column: "ImageVotingId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageVotingEntries_UserId",
                table: "ImageVotingEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageVotingEntryRatings_ImageVotingEntryId",
                table: "ImageVotingEntryRatings",
                column: "ImageVotingEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageVotingEntryRatings_UserId",
                table: "ImageVotingEntryRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageVotings_UploaderUserGroupId",
                table: "ImageVotings",
                column: "UploaderUserGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageVotingChoices");

            migrationBuilder.DropTable(
                name: "ImageVotingEntryRatings");

            migrationBuilder.DropTable(
                name: "ImageVotingEntries");

            migrationBuilder.DropTable(
                name: "ImageVotings");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:grade_type", "regular_grade,behaviour_grade,diligence_grade")
                .Annotation("Npgsql:Enum:lolo_type", "from_grades,from_request")
                .OldAnnotation("Npgsql:Enum:grade_type", "regular_grade,behaviour_grade,diligence_grade")
                .OldAnnotation("Npgsql:Enum:image_voting_type", "single_choice,rating")
                .OldAnnotation("Npgsql:Enum:lolo_type", "from_grades,from_request");
        }
    }
}
