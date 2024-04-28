﻿// <auto-generated />
using System;
using Domain.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CustomSoft.BackEnd.Migrations
{
    [DbContext(typeof(BookMarketContext))]
    [Migration("20240427152812_updatebookaremovefiledirection")]
    partial class updatebookaremovefiledirection
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Book.AcademicGrade", b =>
                {
                    b.Property<int>("AcademicGradeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AcademicGradeId"));

                    b.Property<string>("AcademicCenter")
                        .HasColumnType("text");

                    b.Property<string>("AcademicGradeGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("BookAuthorId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("FechaGrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("AcademicGradeId");

                    b.HasIndex("BookAuthorId");

                    b.ToTable("AcademicGrade");
                });

            modelBuilder.Entity("Domain.Entities.Book.Book", b =>
                {
                    b.Property<Guid>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BookAuthorGuid")
                        .HasColumnType("text");

                    b.Property<byte[]>("File")
                        .HasColumnType("bytea");

                    b.Property<string>("FileExtension")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<string>("FileSize")
                        .HasColumnType("text");

                    b.Property<DateTime?>("PublicationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("BookId");

                    b.ToTable("Book");
                });

            modelBuilder.Entity("Domain.Entities.Book.BookAuthor", b =>
                {
                    b.Property<int>("BookAuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BookAuthorId"));

                    b.Property<DateTime?>("Birthdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("BookAuthorGuid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("BookAuthorId");

                    b.ToTable("BookAuthor");
                });

            modelBuilder.Entity("Domain.Entities.CarritoCompra.CartSession", b =>
                {
                    b.Property<int>("CartSessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CartSessionId"));

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("CartSessionId");

                    b.ToTable("CartSession");
                });

            modelBuilder.Entity("Domain.Entities.ShoppingCart.CartSessionDetail", b =>
                {
                    b.Property<int>("CartSessionDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CartSessionDetailId"));

                    b.Property<int>("CartSessionId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SelectedProduct")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CartSessionDetailId");

                    b.HasIndex("CartSessionId");

                    b.ToTable("CartSessionDetalle");
                });

            modelBuilder.Entity("Domain.Entities.Book.AcademicGrade", b =>
                {
                    b.HasOne("Domain.Entities.Book.BookAuthor", "BookAuthor")
                        .WithMany("AcademicGrades")
                        .HasForeignKey("BookAuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookAuthor");
                });

            modelBuilder.Entity("Domain.Entities.ShoppingCart.CartSessionDetail", b =>
                {
                    b.HasOne("Domain.Entities.CarritoCompra.CartSession", "CartSession")
                        .WithMany("DetailList")
                        .HasForeignKey("CartSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CartSession");
                });

            modelBuilder.Entity("Domain.Entities.Book.BookAuthor", b =>
                {
                    b.Navigation("AcademicGrades");
                });

            modelBuilder.Entity("Domain.Entities.CarritoCompra.CartSession", b =>
                {
                    b.Navigation("DetailList");
                });
#pragma warning restore 612, 618
        }
    }
}
