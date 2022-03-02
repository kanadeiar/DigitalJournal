using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalJournal.Dal.Migrations
{
    public partial class docs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocDirectories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    BaseDirectoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocDirectories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocDirectories_DocDirectories_BaseDirectoryId",
                        column: x => x.BaseDirectoryId,
                        principalTable: "DocDirectories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DocDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Marks = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Note = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    DirectoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocDocuments_DocDirectories_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "DocDirectories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    DocumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocComment_DocDocuments_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "DocDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocComment_DocumentId",
                table: "DocComment",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocDirectories_BaseDirectoryId",
                table: "DocDirectories",
                column: "BaseDirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DocDocuments_DirectoryId",
                table: "DocDocuments",
                column: "DirectoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocComment");

            migrationBuilder.DropTable(
                name: "DocDocuments");

            migrationBuilder.DropTable(
                name: "DocDirectories");
        }
    }
}
