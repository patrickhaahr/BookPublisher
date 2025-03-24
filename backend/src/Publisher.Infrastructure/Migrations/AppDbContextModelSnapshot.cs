﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Publisher.Infrastructure;

#nullable disable

namespace Publisher.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.0-preview.1.25081.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Publisher.Domain.Entities.Book", b =>
                {
                    b.Property<Guid>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("BasePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Genres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mediums")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("BookId");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000010"),
                            BasePrice = 19.99m,
                            Genres = "[2,10]",
                            Mediums = "[1,3]",
                            PublishDate = new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Slug = "the-great-adventure",
                            Title = "The Great Adventure"
                        },
                        new
                        {
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000011"),
                            BasePrice = 24.99m,
                            Genres = "[5,10]",
                            Mediums = "[1]",
                            PublishDate = new DateTime(2023, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Slug = "mystery-of-the-lost-city",
                            Title = "Mystery of the Lost City"
                        },
                        new
                        {
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000012"),
                            BasePrice = 29.99m,
                            Genres = "[4,16]",
                            Mediums = "[3]",
                            PublishDate = new DateTime(2023, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Slug = "future-technologies",
                            Title = "Future Technologies"
                        },
                        new
                        {
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000013"),
                            BasePrice = 14.99m,
                            Genres = "[10,3]",
                            Mediums = "[10,3]",
                            PublishDate = new DateTime(1999, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Slug = "one-piece",
                            Title = "One Piece"
                        });
                });

            modelBuilder.Entity("Publisher.Domain.Entities.BookPersons", b =>
                {
                    b.Property<Guid>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorPersonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BookId", "PersonId");

                    b.HasIndex("AuthorPersonId");

                    b.ToTable("BookPersons", (string)null);

                    b.HasData(
                        new
                        {
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000010"),
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000001"),
                            AuthorPersonId = new Guid("c0a80121-0001-4000-0000-000000000001")
                        },
                        new
                        {
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000011"),
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000002"),
                            AuthorPersonId = new Guid("c0a80121-0001-4000-0000-000000000002")
                        },
                        new
                        {
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000012"),
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000001"),
                            AuthorPersonId = new Guid("c0a80121-0001-4000-0000-000000000001")
                        },
                        new
                        {
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000012"),
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000002"),
                            AuthorPersonId = new Guid("c0a80121-0001-4000-0000-000000000002")
                        });
                });

            modelBuilder.Entity("Publisher.Domain.Entities.Cover", b =>
                {
                    b.Property<Guid>("CoverId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImgBase64")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.HasKey("CoverId");

                    b.HasIndex("BookId");

                    b.ToTable("Covers");

                    b.HasData(
                        new
                        {
                            CoverId = new Guid("c0a80121-0001-4000-0000-000000000020"),
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000010"),
                            CreatedDate = new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImgBase64 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg=="
                        },
                        new
                        {
                            CoverId = new Guid("c0a80121-0001-4000-0000-000000000021"),
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000011"),
                            CreatedDate = new DateTime(2023, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImgBase64 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg=="
                        },
                        new
                        {
                            CoverId = new Guid("c0a80121-0001-4000-0000-000000000022"),
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000012"),
                            CreatedDate = new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImgBase64 = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg=="
                        });
                });

            modelBuilder.Entity("Publisher.Domain.Entities.CoverPersons", b =>
                {
                    b.Property<Guid>("CoverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtistPersonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CoverId", "PersonId");

                    b.HasIndex("ArtistPersonId");

                    b.ToTable("CoverPersons", (string)null);

                    b.HasData(
                        new
                        {
                            CoverId = new Guid("c0a80121-0001-4000-0000-000000000020"),
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000003"),
                            ArtistPersonId = new Guid("c0a80121-0001-4000-0000-000000000003")
                        },
                        new
                        {
                            CoverId = new Guid("c0a80121-0001-4000-0000-000000000021"),
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000004"),
                            ArtistPersonId = new Guid("c0a80121-0001-4000-0000-000000000004")
                        },
                        new
                        {
                            CoverId = new Guid("c0a80121-0001-4000-0000-000000000022"),
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000003"),
                            ArtistPersonId = new Guid("c0a80121-0001-4000-0000-000000000003")
                        },
                        new
                        {
                            CoverId = new Guid("c0a80121-0001-4000-0000-000000000022"),
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000004"),
                            ArtistPersonId = new Guid("c0a80121-0001-4000-0000-000000000004")
                        });
                });

            modelBuilder.Entity("Publisher.Domain.Entities.Person", b =>
                {
                    b.Property<Guid>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PersonType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("PersonId");

                    b.ToTable("Persons");

                    b.HasDiscriminator<string>("PersonType").HasValue("Person");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Publisher.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("user");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("c0a80121-0001-4000-0000-000000000030"),
                            Email = "user1@example.com",
                            PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                            Role = "User",
                            Username = "user1"
                        },
                        new
                        {
                            UserId = new Guid("c0a80121-0001-4000-0000-000000000031"),
                            Email = "user2@example.com",
                            PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                            Role = "User",
                            Username = "user2"
                        });
                });

            modelBuilder.Entity("Publisher.Domain.Entities.UserBookInteraction", b =>
                {
                    b.Property<Guid>("InteractionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSaved")
                        .HasColumnType("bit");

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("InteractionId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("UserBookInteractions", (string)null);

                    b.HasData(
                        new
                        {
                            InteractionId = new Guid("c0a80121-0001-4000-0000-000000000040"),
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000010"),
                            IsFavorite = true,
                            IsSaved = true,
                            Rating = 5,
                            Status = "Read",
                            UserId = new Guid("c0a80121-0001-4000-0000-000000000030")
                        },
                        new
                        {
                            InteractionId = new Guid("c0a80121-0001-4000-0000-000000000041"),
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000011"),
                            IsFavorite = false,
                            IsSaved = true,
                            Status = "Reading",
                            UserId = new Guid("c0a80121-0001-4000-0000-000000000031")
                        },
                        new
                        {
                            InteractionId = new Guid("c0a80121-0001-4000-0000-000000000042"),
                            BookId = new Guid("c0a80121-0001-4000-0000-000000000012"),
                            IsFavorite = true,
                            IsSaved = false,
                            Status = "Want to Read",
                            UserId = new Guid("c0a80121-0001-4000-0000-000000000031")
                        });
                });

            modelBuilder.Entity("Publisher.Domain.Entities.ViewModels.BookSummary", b =>
                {
                    b.Property<double>("AverageRating")
                        .HasColumnType("float");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CompletedCount")
                        .HasColumnType("int");

                    b.Property<int>("DroppedCount")
                        .HasColumnType("int");

                    b.Property<int>("FavoriteCount")
                        .HasColumnType("int");

                    b.Property<int>("OnHoldCount")
                        .HasColumnType("int");

                    b.Property<int>("PlanToReadCount")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable((string)null);

                    b.ToView("vw_BookSummary", (string)null);
                });

            modelBuilder.Entity("Publisher.Domain.Entities.Artist", b =>
                {
                    b.HasBaseType("Publisher.Domain.Entities.Person");

                    b.Property<string>("PortfolioUrl")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasDiscriminator().HasValue("Artist");

                    b.HasData(
                        new
                        {
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000003"),
                            Email = "michael.johnson@example.com",
                            FirstName = "Michael",
                            LastName = "Johnson",
                            Phone = "555-222-3333",
                            PortfolioUrl = "https://portfolio.michaeljohnson.com"
                        },
                        new
                        {
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000004"),
                            Email = "sarah.williams@example.com",
                            FirstName = "Sarah",
                            LastName = "Williams",
                            Phone = "555-444-5555",
                            PortfolioUrl = "https://sarahwilliams-art.com"
                        });
                });

            modelBuilder.Entity("Publisher.Domain.Entities.Author", b =>
                {
                    b.HasBaseType("Publisher.Domain.Entities.Person");

                    b.Property<decimal>("RoyaltyRate")
                        .HasColumnType("decimal(5,2)");

                    b.HasDiscriminator().HasValue("Author");

                    b.HasData(
                        new
                        {
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000001"),
                            Email = "john.doe@example.com",
                            FirstName = "John",
                            LastName = "Doe",
                            Phone = "555-123-4567",
                            RoyaltyRate = 15.5m
                        },
                        new
                        {
                            PersonId = new Guid("c0a80121-0001-4000-0000-000000000002"),
                            Email = "jane.smith@example.com",
                            FirstName = "Jane",
                            LastName = "Smith",
                            Phone = "555-987-6543",
                            RoyaltyRate = 12.75m
                        });
                });

            modelBuilder.Entity("Publisher.Domain.Entities.BookPersons", b =>
                {
                    b.HasOne("Publisher.Domain.Entities.Author", "Author")
                        .WithMany("BookPersons")
                        .HasForeignKey("AuthorPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Publisher.Domain.Entities.Book", "Book")
                        .WithMany("BookPersons")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Publisher.Domain.Entities.Cover", b =>
                {
                    b.HasOne("Publisher.Domain.Entities.Book", "Book")
                        .WithMany("Covers")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Publisher.Domain.Entities.CoverPersons", b =>
                {
                    b.HasOne("Publisher.Domain.Entities.Artist", "Artist")
                        .WithMany("CoverPersons")
                        .HasForeignKey("ArtistPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Publisher.Domain.Entities.Cover", "Cover")
                        .WithMany("CoverPersons")
                        .HasForeignKey("CoverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Cover");
                });

            modelBuilder.Entity("Publisher.Domain.Entities.UserBookInteraction", b =>
                {
                    b.HasOne("Publisher.Domain.Entities.Book", "Book")
                        .WithMany("UserBookInteractions")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Publisher.Domain.Entities.User", "User")
                        .WithMany("UserBookInteractions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Publisher.Domain.Entities.Book", b =>
                {
                    b.Navigation("BookPersons");

                    b.Navigation("Covers");

                    b.Navigation("UserBookInteractions");
                });

            modelBuilder.Entity("Publisher.Domain.Entities.Cover", b =>
                {
                    b.Navigation("CoverPersons");
                });

            modelBuilder.Entity("Publisher.Domain.Entities.User", b =>
                {
                    b.Navigation("UserBookInteractions");
                });

            modelBuilder.Entity("Publisher.Domain.Entities.Artist", b =>
                {
                    b.Navigation("CoverPersons");
                });

            modelBuilder.Entity("Publisher.Domain.Entities.Author", b =>
                {
                    b.Navigation("BookPersons");
                });
#pragma warning restore 612, 618
        }
    }
}
