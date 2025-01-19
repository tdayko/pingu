﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pingu.Infrastructure.Data.Context;

#nullable disable

namespace Pingu.Infrastructure.Migrations
{
    [DbContext(typeof(PinguDbContext))]
    [Migration("20250119021248_AddNewMigrations")]
    partial class AddNewMigrations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Pingu.Core.Domain.Entities.ShortenedUrl", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("OriginalUrl")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("ShortCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("ShortCode")
                        .IsUnique();

                    b.ToTable("ShortenedUrls");
                });

            modelBuilder.Entity("Pingu.Core.Domain.Entities.UrlStatistics", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("FirstClickedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastClickedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ShortenedUrlId")
                        .HasColumnType("uuid");

                    b.Property<int>("TotalClicks")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("LastClickedAt");

                    b.HasIndex("ShortenedUrlId")
                        .IsUnique();

                    b.ToTable("UrlStatistics");
                });

            modelBuilder.Entity("Pingu.Core.Domain.Entities.UrlStatistics", b =>
                {
                    b.HasOne("Pingu.Core.Domain.Entities.ShortenedUrl", "ShortenedUrl")
                        .WithOne("Statistics")
                        .HasForeignKey("Pingu.Core.Domain.Entities.UrlStatistics", "ShortenedUrlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShortenedUrl");
                });

            modelBuilder.Entity("Pingu.Core.Domain.Entities.ShortenedUrl", b =>
                {
                    b.Navigation("Statistics")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
