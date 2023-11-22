using System.Collections.Generic;
using Blueboard.Infrastructure.Persistence.Entities.Owned;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blueboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Owned_Json_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Inputs",
                table: "Products",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(List<ProductInput>),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "Aspects",
                table: "ImageVotings",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(List<ImageVotingAspect>),
                oldType: "jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<ProductInput>>(
                name: "Inputs",
                table: "Products",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<ImageVotingAspect>>(
                name: "Aspects",
                table: "ImageVotings",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);
        }
    }
}
