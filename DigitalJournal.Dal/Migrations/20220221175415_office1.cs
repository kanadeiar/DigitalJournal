using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalJournal.Dal.Migrations
{
    public partial class office1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Office1Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office1Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Office1Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SurName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Patronymic = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Office1PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Assembler = table.Column<int>(type: "INTEGER", nullable: false),
                    CCpp = table.Column<int>(type: "INTEGER", nullable: false),
                    CSharp = table.Column<int>(type: "INTEGER", nullable: false),
                    Java = table.Column<int>(type: "INTEGER", nullable: false),
                    PHP = table.Column<int>(type: "INTEGER", nullable: false),
                    SQL = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office1Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Office1Skills_Office1Positions_Office1PositionId",
                        column: x => x.Office1PositionId,
                        principalTable: "Office1Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Office1Skills_Office1PositionId",
                table: "Office1Skills",
                column: "Office1PositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Office1Skills");

            migrationBuilder.DropTable(
                name: "Office1Positions");
        }
    }
}
