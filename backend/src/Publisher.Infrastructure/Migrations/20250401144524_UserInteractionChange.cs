using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Publisher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserInteractionChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-1234-567890abcdef"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE+pR7mKHIeV7jSJTyEFpDuudDUsog15tYqUh88nh9RXUQS8EIaQVm193gX1fUwf3A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("c0a80121-0001-4000-0000-000000000030"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN75+ygtno1KZTlJMlXQenrP5q5dW514mhjhWRnhsQ7tqnoiKWLnLdd8QzJh1/ZGow==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("c0a80121-0001-4000-0000-000000000031"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGfoEFbc6vWNYizMG9O9jpOTmjZAdba42h4I496XhQd87tqpFnUl+dpUijXU8f8v3g==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-1234-567890abcdef"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECRFE8ENk/5NsdXwEvabqJj6sAK1JGDF5I+54TzK2tkbCzEjTA9hgcWGG5xvh7HVtg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("c0a80121-0001-4000-0000-000000000030"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOJstwSoKOOddW2A+uVTcXqRIp0SKbYwvf5PJIFbzhOqMM/rpzdd4iEo8g/WJDZlCw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("c0a80121-0001-4000-0000-000000000031"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEErjvO/TvZ2wbXGkfG0ea8tV3Cc5mH4Xxg5z/kiA3/8OrOlf8Zef+JRpjhcnURI3RQ==");
        }
    }
}
