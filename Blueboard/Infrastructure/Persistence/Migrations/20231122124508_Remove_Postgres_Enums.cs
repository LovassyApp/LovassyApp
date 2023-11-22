using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blueboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Postgres_Enums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:grade_type", "regular_grade,behaviour_grade,diligence_grade")
                .OldAnnotation("Npgsql:Enum:image_voting_type", "single_choice,increment")
                .OldAnnotation("Npgsql:Enum:lolo_type", "from_grades,from_request");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:grade_type", "regular_grade,behaviour_grade,diligence_grade")
                .Annotation("Npgsql:Enum:image_voting_type", "single_choice,increment")
                .Annotation("Npgsql:Enum:lolo_type", "from_grades,from_request");
        }
    }
}
