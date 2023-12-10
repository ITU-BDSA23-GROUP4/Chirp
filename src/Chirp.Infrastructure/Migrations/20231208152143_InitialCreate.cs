using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "AuthorAuthor",
                columns: table => new
                {
                    FollowedAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowersAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorAuthor", x => new { x.FollowedAuthorId, x.FollowersAuthorId });
                    table.ForeignKey(
                        name: "FK_AuthorAuthor_Authors_FollowedAuthorId",
                        column: x => x.FollowedAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorAuthor_Authors_FollowersAuthorId",
                        column: x => x.FollowersAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId");
                });

            migrationBuilder.CreateTable(
                name: "Cheeps",
                columns: table => new
                {
                    CheepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cheeps", x => x.CheepId);
                    table.ForeignKey(
                        name: "FK_Cheeps_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorAuthor_FollowersAuthorId",
                table: "AuthorAuthor",
                column: "FollowersAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cheeps_AuthorId",
                table: "Cheeps",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorAuthor");

            migrationBuilder.DropTable(
                name: "Cheeps");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
