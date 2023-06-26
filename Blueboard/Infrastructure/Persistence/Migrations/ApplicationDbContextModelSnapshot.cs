﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities.Owned;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace Blueboard.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "grade_type", new[] { "regular_grade", "behaviour_grade", "diligence_grade" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "lolo_type", new[] { "from_grades", "from_request" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EvaluationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("GradeType")
                        .HasColumnType("integer");

                    b.Property<int>("GradeValue")
                        .HasColumnType("integer");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LoloIdHashed")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShortTextGrade")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SubjectCategory")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Teacher")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TextGrade")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Uid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserIdHashed")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.GradeImport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("JsonEncrypted")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("GradeImports");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.ImportKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<string>("KeyHashed")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("KeyProtected")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("KeyHashed")
                        .IsUnique();

                    b.ToTable("ImportKeys");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.Lolo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsSpent")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int>("LoloType")
                        .HasColumnType("integer");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Lolos");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.LoloRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AcceptedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DeniedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("LoloRequests");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.OwnedItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("UsedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("OwnedItems");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.OwnedItemUse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("OwnedItemId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Dictionary<string, string>>("Values")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("OwnedItemId");

                    b.ToTable("OwnedItemUses");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.PersonalAccessToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUsedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("PersonalAccessTokens");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<ProductInput>>("Inputs")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string[]>("NotifiedEmails")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<bool>("QRCodeActivated")
                        .HasColumnType("boolean");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("RichTextContent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<NpgsqlTsVector>("SearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "hungarian")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "Name", "Description", "RichTextContent" });

                    b.Property<string>("ThumbnailUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Visible")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("SearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SearchVector"), "GIN");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.QRCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Secret")
                        .IsUnique();

                    b.ToTable("QRCodes");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.ResetKeyPasswordSetNotifier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("ResetKeyPasswordSetNotifiers");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.StoreHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("LolosSpent")
                        .HasColumnType("integer");

                    b.Property<int?>("OwnedItemId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("ProductId")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OwnedItemId")
                        .IsUnique();

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("StoreHistories");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Class")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("EmailVerifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("HasherSaltEncrypted")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HasherSaltHashed")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("ImportAvailable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("MasterKeyEncrypted")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MasterKeySalt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("OmCodeEncrypted")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OmCodeHashed")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHashed")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PrivateKeyEncrypted")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RealName")
                        .HasColumnType("text");

                    b.Property<string>("ResetKeyEncrypted")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("HasherSaltHashed")
                        .IsUnique();

                    b.HasIndex("MasterKeySalt")
                        .IsUnique();

                    b.HasIndex("OmCodeHashed")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.UserGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string[]>("Permissions")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("ProductQRCode", b =>
                {
                    b.Property<int>("ProductsId")
                        .HasColumnType("integer");

                    b.Property<int>("QRCodesId")
                        .HasColumnType("integer");

                    b.HasKey("ProductsId", "QRCodesId");

                    b.HasIndex("QRCodesId");

                    b.ToTable("ProductQRCode");
                });

            modelBuilder.Entity("UserUserGroup", b =>
                {
                    b.Property<int>("UserGroupsId")
                        .HasColumnType("integer");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("UserGroupsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserUserGroup");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.GradeImport", b =>
                {
                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("GradeImports")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.Lolo", b =>
                {
                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("Lolos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.LoloRequest", b =>
                {
                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("LoloRequests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.OwnedItem", b =>
                {
                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.Product", "Product")
                        .WithMany("OwnedItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("OwnedItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.OwnedItemUse", b =>
                {
                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.OwnedItem", "OwnedItem")
                        .WithMany("OwnedItemUses")
                        .HasForeignKey("OwnedItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OwnedItem");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.PersonalAccessToken", b =>
                {
                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("PersonalAccessTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.StoreHistory", b =>
                {
                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.OwnedItem", "OwnedItem")
                        .WithOne("StoreHistory")
                        .HasForeignKey("Blueboard.Infrastructure.Persistence.Entities.StoreHistory", "OwnedItemId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.Product", "Product")
                        .WithMany("StoreHistories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("StoreHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OwnedItem");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProductQRCode", b =>
                {
                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.QRCode", null)
                        .WithMany()
                        .HasForeignKey("QRCodesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserUserGroup", b =>
                {
                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.UserGroup", null)
                        .WithMany()
                        .HasForeignKey("UserGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blueboard.Infrastructure.Persistence.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.OwnedItem", b =>
                {
                    b.Navigation("OwnedItemUses");

                    b.Navigation("StoreHistory")
                        .IsRequired();
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.Product", b =>
                {
                    b.Navigation("OwnedItems");

                    b.Navigation("StoreHistories");
                });

            modelBuilder.Entity("Blueboard.Infrastructure.Persistence.Entities.User", b =>
                {
                    b.Navigation("GradeImports");

                    b.Navigation("LoloRequests");

                    b.Navigation("Lolos");

                    b.Navigation("OwnedItems");

                    b.Navigation("PersonalAccessTokens");

                    b.Navigation("StoreHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
