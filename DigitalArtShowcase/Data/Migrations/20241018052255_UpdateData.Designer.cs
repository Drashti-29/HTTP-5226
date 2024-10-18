﻿// <auto-generated />
using System;
using DigitalArtShowcase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DigitalArtShowcase.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241018052255_UpdateData")]
    partial class UpdateData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ArtworkExhibition", b =>
                {
                    b.Property<int>("ArtworksArtworkId")
                        .HasColumnType("int");

                    b.Property<int>("ExhibitionsExhibitionId")
                        .HasColumnType("int");

                    b.HasKey("ArtworksArtworkId", "ExhibitionsExhibitionId");

                    b.HasIndex("ExhibitionsExhibitionId");

                    b.ToTable("ArtworkExhibition");
                });

            modelBuilder.Entity("DigitalArtShowcase.Models.Artist", b =>
                {
                    b.Property<int>("ArtistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArtistId"));

                    b.Property<string>("ArtistBio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArtistId");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("DigitalArtShowcase.Models.Artwork", b =>
                {
                    b.Property<int>("ArtworkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArtworkId"));

                    b.Property<int>("ArtistId")
                        .HasColumnType("int");

                    b.Property<int>("CreationYear")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArtworkId");

                    b.HasIndex("ArtistId");

                    b.ToTable("Artworks");
                });

            modelBuilder.Entity("DigitalArtShowcase.Models.Exhibition", b =>
                {
                    b.Property<int>("ExhibitionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExhibitionId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExhibitionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ExhibitionId");

                    b.ToTable("Exhibitions");
                });

            modelBuilder.Entity("ArtworkExhibition", b =>
                {
                    b.HasOne("DigitalArtShowcase.Models.Artwork", null)
                        .WithMany()
                        .HasForeignKey("ArtworksArtworkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalArtShowcase.Models.Exhibition", null)
                        .WithMany()
                        .HasForeignKey("ExhibitionsExhibitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DigitalArtShowcase.Models.Artwork", b =>
                {
                    b.HasOne("DigitalArtShowcase.Models.Artist", "Artist")
                        .WithMany("Artworks")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("DigitalArtShowcase.Models.Artist", b =>
                {
                    b.Navigation("Artworks");
                });
#pragma warning restore 612, 618
        }
    }
}
