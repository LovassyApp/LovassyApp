using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace Blueboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Use_Hungarian_Collation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("SearchVector", "Products");
            
            migrationBuilder.AlterColumn<string>(
                name: "ResetKeyEncrypted",
                table: "Users",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RealName",
                table: "Users",
                type: "text",
                nullable: true,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PublicKey",
                table: "Users",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PrivateKeyEncrypted",
                table: "Users",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHashed",
                table: "Users",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "OmCodeHashed",
                table: "Users",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "OmCodeEncrypted",
                table: "Users",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "MasterKeySalt",
                table: "Users",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MasterKeyEncrypted",
                table: "Users",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "HasherSaltHashed",
                table: "Users",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "HasherSaltEncrypted",
                table: "Users",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Class",
                table: "Users",
                type: "text",
                nullable: true,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserGroups",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "StoreHistories",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ResetKeyPasswordSetNotifiers",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Secret",
                table: "QRCodes",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QRCodes",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "QRCodes",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ThumbnailUrl",
                table: "Products",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RichTextContent",
                table: "Products",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "PersonalAccessTokens",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Lolos",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "LoloRequests",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "LoloRequests",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "LoloRequestCreatedNotifiers",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ImportKeys",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "KeyProtected",
                table: "ImportKeys",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "KeyHashed",
                table: "ImportKeys",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ImageVotings",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ImageVotings",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AspectKey",
                table: "ImageVotingEntryIncrements",
                type: "text",
                nullable: true,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ImageVotingEntries",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ImageVotingEntries",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AspectKey",
                table: "ImageVotingChoices",
                type: "text",
                nullable: true,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserIdHashed",
                table: "Grades",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Uid",
                table: "Grades",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Grades",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Theme",
                table: "Grades",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "TextGrade",
                table: "Grades",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Teacher",
                table: "Grades",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectCategory",
                table: "Grades",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Grades",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ShortTextGrade",
                table: "Grades",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LoloIdHashed",
                table: "Grades",
                type: "text",
                nullable: true,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Group",
                table: "Grades",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "JsonEncrypted",
                table: "GradeImports",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "FileUploads",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "FileUploads",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalFilename",
                table: "FileUploads",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "FileUploads",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                table: "FileUploads",
                type: "text",
                nullable: false,
                collation: "hu_HU",
                oldClrType: typeof(string),
                oldType: "text");
            
            migrationBuilder.AddColumn<NpgsqlTsVector>("SearchVector", "Products", type: "tsvector", nullable: false)
                .Annotation("Npgsql:TsVectorConfig", "hungarian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name", "Description", "RichTextContent" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("SearchVector", "Products");
            
            migrationBuilder.AlterColumn<string>(
                name: "ResetKeyEncrypted",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "RealName",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "PublicKey",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "PrivateKeyEncrypted",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHashed",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "OmCodeHashed",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "OmCodeEncrypted",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "MasterKeySalt",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "MasterKeyEncrypted",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "HasherSaltHashed",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "HasherSaltEncrypted",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Class",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserGroups",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "StoreHistories",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ResetKeyPasswordSetNotifiers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Secret",
                table: "QRCodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QRCodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "QRCodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "ThumbnailUrl",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "RichTextContent",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "PersonalAccessTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Lolos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "LoloRequests",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "LoloRequests",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "LoloRequestCreatedNotifiers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ImportKeys",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "KeyProtected",
                table: "ImportKeys",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "KeyHashed",
                table: "ImportKeys",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ImageVotings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ImageVotings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "AspectKey",
                table: "ImageVotingEntryIncrements",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ImageVotingEntries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ImageVotingEntries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "AspectKey",
                table: "ImageVotingChoices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "UserIdHashed",
                table: "Grades",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Uid",
                table: "Grades",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Grades",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Theme",
                table: "Grades",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "TextGrade",
                table: "Grades",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Teacher",
                table: "Grades",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectCategory",
                table: "Grades",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Grades",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "ShortTextGrade",
                table: "Grades",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "LoloIdHashed",
                table: "Grades",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Group",
                table: "Grades",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "JsonEncrypted",
                table: "GradeImports",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "FileUploads",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "FileUploads",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalFilename",
                table: "FileUploads",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "FileUploads",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");

            migrationBuilder.AlterColumn<string>(
                name: "Filename",
                table: "FileUploads",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "hu_HU");
            
            migrationBuilder.AddColumn<NpgsqlTsVector>("SearchVector", "Products", type: "tsvector", nullable: false)
                .Annotation("Npgsql:TsVectorConfig", "hungarian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name", "Description", "RichTextContent" });
        }
    }
}
