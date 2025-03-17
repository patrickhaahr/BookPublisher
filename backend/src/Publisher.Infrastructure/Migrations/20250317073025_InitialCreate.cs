using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Publisher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PersonType = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    PortfolioUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RoyaltyRate = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "user")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Covers",
                columns: table => new
                {
                    CoverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImgBase64 = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Covers", x => x.CoverId);
                    table.ForeignKey(
                        name: "FK_Covers_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookGenres",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenres", x => new { x.BookId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_BookGenres_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookPersons",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookPersons", x => new { x.BookId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_BookPersons_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookPersons_Persons_AuthorPersonId",
                        column: x => x.AuthorPersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBookInteractions",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsFavorite = table.Column<bool>(type: "bit", nullable: false),
                    IsSaved = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBookInteractions", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_UserBookInteractions_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBookInteractions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoverPersons",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverPersons", x => new { x.CoverId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_CoverPersons_Covers_CoverId",
                        column: x => x.CoverId,
                        principalTable: "Covers",
                        principalColumn: "CoverId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoverPersons_Persons_ArtistPersonId",
                        column: x => x.ArtistPersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "BasePrice", "PublishDate", "Slug", "Title" },
                values: new object[,]
                {
                    { new Guid("c0a80121-0001-4000-0000-000000000010"), 19.99m, new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "the-great-adventure", "The Great Adventure" },
                    { new Guid("c0a80121-0001-4000-0000-000000000011"), 24.99m, new DateTime(2023, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "mystery-of-the-lost-city", "Mystery of the Lost City" },
                    { new Guid("c0a80121-0001-4000-0000-000000000012"), 29.99m, new DateTime(2023, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "future-technologies", "Future Technologies" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "Name" },
                values: new object[,]
                {
                    { 1, "Science Fiction" },
                    { 2, "Mystery" },
                    { 3, "Fantasy" },
                    { 4, "Non-Fiction" },
                    { 5, "Biography" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Email", "FirstName", "LastName", "PersonType", "Phone", "RoyaltyRate" },
                values: new object[,]
                {
                    { new Guid("c0a80121-0001-4000-0000-000000000001"), "john.doe@example.com", "John", "Doe", "Author", "555-123-4567", 15.5m },
                    { new Guid("c0a80121-0001-4000-0000-000000000002"), "jane.smith@example.com", "Jane", "Smith", "Author", "555-987-6543", 12.75m }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Email", "FirstName", "LastName", "PersonType", "Phone", "PortfolioUrl" },
                values: new object[,]
                {
                    { new Guid("c0a80121-0001-4000-0000-000000000003"), "michael.johnson@example.com", "Michael", "Johnson", "Artist", "555-222-3333", "https://portfolio.michaeljohnson.com" },
                    { new Guid("c0a80121-0001-4000-0000-000000000004"), "sarah.williams@example.com", "Sarah", "Williams", "Artist", "555-444-5555", "https://sarahwilliams-art.com" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("c0a80121-0001-4000-0000-000000000030"), "user1@example.com", "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8", "User", "user1" },
                    { new Guid("c0a80121-0001-4000-0000-000000000031"), "user2@example.com", "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8", "User", "user2" }
                });

            migrationBuilder.InsertData(
                table: "BookGenres",
                columns: new[] { "BookId", "GenreId" },
                values: new object[,]
                {
                    { new Guid("c0a80121-0001-4000-0000-000000000010"), 3 },
                    { new Guid("c0a80121-0001-4000-0000-000000000011"), 2 },
                    { new Guid("c0a80121-0001-4000-0000-000000000012"), 1 },
                    { new Guid("c0a80121-0001-4000-0000-000000000012"), 4 }
                });

            migrationBuilder.InsertData(
                table: "BookPersons",
                columns: new[] { "BookId", "PersonId", "AuthorPersonId" },
                values: new object[,]
                {
                    { new Guid("c0a80121-0001-4000-0000-000000000010"), new Guid("c0a80121-0001-4000-0000-000000000001"), new Guid("c0a80121-0001-4000-0000-000000000001") },
                    { new Guid("c0a80121-0001-4000-0000-000000000011"), new Guid("c0a80121-0001-4000-0000-000000000002"), new Guid("c0a80121-0001-4000-0000-000000000002") },
                    { new Guid("c0a80121-0001-4000-0000-000000000012"), new Guid("c0a80121-0001-4000-0000-000000000001"), new Guid("c0a80121-0001-4000-0000-000000000001") },
                    { new Guid("c0a80121-0001-4000-0000-000000000012"), new Guid("c0a80121-0001-4000-0000-000000000002"), new Guid("c0a80121-0001-4000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "Covers",
                columns: new[] { "CoverId", "BookId", "CreatedDate", "ImgBase64" },
                values: new object[,]
                {
                    { new Guid("c0a80121-0001-4000-0000-000000000020"), new Guid("c0a80121-0001-4000-0000-000000000010"), new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==" },
                    { new Guid("c0a80121-0001-4000-0000-000000000021"), new Guid("c0a80121-0001-4000-0000-000000000011"), new DateTime(2023, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==" },
                    { new Guid("c0a80121-0001-4000-0000-000000000022"), new Guid("c0a80121-0001-4000-0000-000000000012"), new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==" }
                });

            migrationBuilder.InsertData(
                table: "UserBookInteractions",
                columns: new[] { "InteractionId", "BookId", "IsFavorite", "IsSaved", "Rating", "Status", "UserId" },
                values: new object[,]
                {
                    { new Guid("c0a80121-0001-4000-0000-000000000040"), new Guid("c0a80121-0001-4000-0000-000000000010"), true, true, 5, "Read", new Guid("c0a80121-0001-4000-0000-000000000030") },
                    { new Guid("c0a80121-0001-4000-0000-000000000041"), new Guid("c0a80121-0001-4000-0000-000000000011"), false, true, null, "Reading", new Guid("c0a80121-0001-4000-0000-000000000031") },
                    { new Guid("c0a80121-0001-4000-0000-000000000042"), new Guid("c0a80121-0001-4000-0000-000000000012"), true, false, null, "Want to Read", new Guid("c0a80121-0001-4000-0000-000000000031") }
                });

            migrationBuilder.InsertData(
                table: "CoverPersons",
                columns: new[] { "CoverId", "PersonId", "ArtistPersonId" },
                values: new object[,]
                {
                    { new Guid("c0a80121-0001-4000-0000-000000000020"), new Guid("c0a80121-0001-4000-0000-000000000003"), new Guid("c0a80121-0001-4000-0000-000000000003") },
                    { new Guid("c0a80121-0001-4000-0000-000000000021"), new Guid("c0a80121-0001-4000-0000-000000000004"), new Guid("c0a80121-0001-4000-0000-000000000004") },
                    { new Guid("c0a80121-0001-4000-0000-000000000022"), new Guid("c0a80121-0001-4000-0000-000000000003"), new Guid("c0a80121-0001-4000-0000-000000000003") },
                    { new Guid("c0a80121-0001-4000-0000-000000000022"), new Guid("c0a80121-0001-4000-0000-000000000004"), new Guid("c0a80121-0001-4000-0000-000000000004") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_GenreId",
                table: "BookGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_BookPersons_AuthorPersonId",
                table: "BookPersons",
                column: "AuthorPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Slug",
                table: "Books",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoverPersons_ArtistPersonId",
                table: "CoverPersons",
                column: "ArtistPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Covers_BookId",
                table: "Covers",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookInteractions_BookId",
                table: "UserBookInteractions",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookInteractions_UserId",
                table: "UserBookInteractions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookGenres");

            migrationBuilder.DropTable(
                name: "BookPersons");

            migrationBuilder.DropTable(
                name: "CoverPersons");

            migrationBuilder.DropTable(
                name: "UserBookInteractions");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Covers");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
