﻿// <auto-generated />
using System;
using API.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(ContextApi))]
    [Migration("20220115122954_gamedemandNavigation")]
    partial class gamedemandNavigation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("keys_exchange")
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("API.Models.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("AdminComment")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("admin_comment");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("created_by");

                    b.Property<DateTime?>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("created_on")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("deleted_by");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_on");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<bool?>("GeneratedInfo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("generated_info")
                        .HasDefaultValueSql("0");

                    b.Property<DateTime?>("GivenDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("given_date");

                    b.Property<string>("GivenTo")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("given_to");

                    b.Property<bool>("IsAvailable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("is_available")
                        .HasDefaultValueSql("1");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("key");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("last_modified_on")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("link");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("ObtenaidBy")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("obtenaid_by");

                    b.Property<string>("OwnerComment")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("owner_comment");

                    b.Property<string>("Platforme")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("platforme");

                    b.Property<string>("Price")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("price");

                    b.Property<string>("PublicComment")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("public_comment");

                    b.Property<DateTime>("ReceivedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("received_date");

                    b.Property<string>("Reviews")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("reviews");

                    b.Property<bool>("SoftDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("soft_deleted")
                        .HasDefaultValueSql("0");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("title");

                    b.Property<string>("TumbnailUrl")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("tumbnail_url");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_games");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_games_user_id");

                    b.ToTable("games", "keys_exchange");
                });

            modelBuilder.Entity("API.Models.GameDemand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<bool>("Approuved")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("approuved")
                        .HasDefaultValueSql("0");

                    b.Property<string>("ContactInfo")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("contact_info");

                    b.Property<string>("ContactName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("contact_name");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("created_by");

                    b.Property<DateTime?>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("created_on")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("deleted_by");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_on");

                    b.Property<Guid?>("GameId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("game_id");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("last_modified_on")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("SoftDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("soft_deleted")
                        .HasDefaultValueSql("0");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_game_demands");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_game_demands_user_id");

                    b.ToTable("game_demands", "keys_exchange");
                });

            modelBuilder.Entity("API.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("created_by");

                    b.Property<DateTime?>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("created_on")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("deleted_by");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_on");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("key");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("last_modified_on")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<bool>("SoftDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("soft_deleted")
                        .HasDefaultValueSql("0");

                    b.Property<DateTime?>("ValidUpto")
                        .HasColumnType("datetime2")
                        .HasColumnName("valid_upto");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles", "keys_exchange");
                });

            modelBuilder.Entity("API.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int")
                        .HasColumnName("access_failed_count");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("created_by");

                    b.Property<DateTime?>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("created_on")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("deleted_by");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_on");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<string>("Discord")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("discord");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit")
                        .HasColumnName("is_public");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime2")
                        .HasColumnName("last_login");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("last_modified_by");

                    b.Property<DateTime?>("LastModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("last_modified_on")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("last_name");

                    b.Property<string>("OtherLink")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("other_link");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("phone_number");

                    b.Property<string>("PictureUri")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("picture_uri");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("role_id");

                    b.Property<bool>("ShowDiscord")
                        .HasColumnType("bit")
                        .HasColumnName("show_discord");

                    b.Property<bool>("ShowEmail")
                        .HasColumnType("bit")
                        .HasColumnName("show_email");

                    b.Property<bool>("ShowFirstName")
                        .HasColumnType("bit")
                        .HasColumnName("show_first_name");

                    b.Property<bool>("ShowLastName")
                        .HasColumnType("bit")
                        .HasColumnName("show_last_name");

                    b.Property<bool>("ShowPhoneNumber")
                        .HasColumnType("bit")
                        .HasColumnName("show_phone_number");

                    b.Property<bool>("SoftDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("soft_deleted")
                        .HasDefaultValueSql("0");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_application_users");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_application_users_role_id");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("ix_application_users_username");

                    b.ToTable("application_users", "keys_exchange");
                });

            modelBuilder.Entity("API.Models.Game", b =>
                {
                    b.HasOne("API.Models.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_games_application_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Models.GameDemand", b =>
                {
                    b.HasOne("API.Models.Game", "Game")
                        .WithMany("GameDemands")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_game_demands_games_game_id");

                    b.HasOne("API.Models.User", "User")
                        .WithMany("GameDemands")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_game_demands_application_users_user_id");

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Models.User", b =>
                {
                    b.HasOne("API.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_application_users_roles_role_id");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("API.Models.Game", b =>
                {
                    b.Navigation("GameDemands");
                });

            modelBuilder.Entity("API.Models.User", b =>
                {
                    b.Navigation("GameDemands");

                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
