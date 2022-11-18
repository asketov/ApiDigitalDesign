﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApiDigitalDesign.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221118081545_addLikeEntities")]
    partial class addLikeEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entities.Attach", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Attaches");
                });

            modelBuilder.Entity("DAL.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("DAL.Entities.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("DAL.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DAL.Entities.UserSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("RefreshTokenId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("DAL.Entities.Avatar", b =>
                {
                    b.HasBaseType("DAL.Entities.Attach");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Avatars", (string)null);
                });

            modelBuilder.Entity("DAL.Entities.CommentLike", b =>
                {
                    b.HasBaseType("DAL.Entities.Like");

                    b.Property<Guid>("CommentId")
                        .HasColumnType("uuid");

                    b.HasIndex("CommentId");

                    b.ToTable("CommentLikes", (string)null);
                });

            modelBuilder.Entity("DAL.Entities.PostAttach", b =>
                {
                    b.HasBaseType("DAL.Entities.Attach");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.HasIndex("PostId");

                    b.ToTable("PostAttaches", (string)null);
                });

            modelBuilder.Entity("DAL.Entities.PostLike", b =>
                {
                    b.HasBaseType("DAL.Entities.Like");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.HasIndex("PostId");

                    b.ToTable("PostLikes", (string)null);
                });

            modelBuilder.Entity("DAL.Entities.Attach", b =>
                {
                    b.HasOne("DAL.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("DAL.Entities.Comment", b =>
                {
                    b.HasOne("DAL.Entities.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("DAL.Entities.Like", b =>
                {
                    b.HasOne("DAL.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("DAL.Entities.Post", b =>
                {
                    b.HasOne("DAL.Entities.User", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("DAL.Entities.UserSession", b =>
                {
                    b.HasOne("DAL.Entities.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DAL.Entities.Avatar", b =>
                {
                    b.HasOne("DAL.Entities.Attach", null)
                        .WithOne()
                        .HasForeignKey("DAL.Entities.Avatar", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.User", "User")
                        .WithOne("Avatar")
                        .HasForeignKey("DAL.Entities.Avatar", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DAL.Entities.CommentLike", b =>
                {
                    b.HasOne("DAL.Entities.Comment", "Comment")
                        .WithMany("Likes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Like", null)
                        .WithOne()
                        .HasForeignKey("DAL.Entities.CommentLike", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("DAL.Entities.PostAttach", b =>
                {
                    b.HasOne("DAL.Entities.Attach", null)
                        .WithOne()
                        .HasForeignKey("DAL.Entities.PostAttach", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Post", "Post")
                        .WithMany("PostAttaches")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("DAL.Entities.PostLike", b =>
                {
                    b.HasOne("DAL.Entities.Like", null)
                        .WithOne()
                        .HasForeignKey("DAL.Entities.PostLike", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("DAL.Entities.Comment", b =>
                {
                    b.Navigation("Likes");
                });

            modelBuilder.Entity("DAL.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("PostAttaches");
                });

            modelBuilder.Entity("DAL.Entities.User", b =>
                {
                    b.Navigation("Avatar");

                    b.Navigation("Comments");

                    b.Navigation("Posts");

                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
