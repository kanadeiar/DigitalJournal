using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalJournal.Dal.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Factory1ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Units = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factory1ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Factory1Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factory1Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SurName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Patronymic = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Factory1Autoclave1ShiftDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AutoclaveNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AutoclavedTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Factory1ProductTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    AutoclavedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Factory1ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factory1Autoclave1ShiftDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factory1Autoclave1ShiftDatas_Factory1ProductTypes_Factory1ProductTypeId",
                        column: x => x.Factory1ProductTypeId,
                        principalTable: "Factory1ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1Autoclave1ShiftDatas_Factory1Shifts_Factory1ShiftId",
                        column: x => x.Factory1ShiftId,
                        principalTable: "Factory1Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1Autoclave1ShiftDatas_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factory1GeneralShiftData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Factory1ProductTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Loose1RawValue = table.Column<double>(type: "REAL", nullable: false),
                    Loose2RawValue = table.Column<double>(type: "REAL", nullable: false),
                    Loose3RawValue = table.Column<double>(type: "REAL", nullable: false),
                    AutoclaveNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Factory1PackProductTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    PackProductCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Factory1ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factory1GeneralShiftData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factory1GeneralShiftData_Factory1ProductTypes_Factory1PackProductTypeId",
                        column: x => x.Factory1PackProductTypeId,
                        principalTable: "Factory1ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1GeneralShiftData_Factory1ProductTypes_Factory1ProductTypeId",
                        column: x => x.Factory1ProductTypeId,
                        principalTable: "Factory1ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1GeneralShiftData_Factory1Shifts_Factory1ShiftId",
                        column: x => x.Factory1ShiftId,
                        principalTable: "Factory1Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1GeneralShiftData_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factory1Pack1ShiftDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Factory1ProductTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Factory1ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factory1Pack1ShiftDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factory1Pack1ShiftDatas_Factory1ProductTypes_Factory1ProductTypeId",
                        column: x => x.Factory1ProductTypeId,
                        principalTable: "Factory1ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1Pack1ShiftDatas_Factory1Shifts_Factory1ShiftId",
                        column: x => x.Factory1ShiftId,
                        principalTable: "Factory1Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1Pack1ShiftDatas_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factory1Press1ShiftData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Factory1ProductTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Loose1RawValue = table.Column<double>(type: "REAL", nullable: false),
                    Loose2RawValue = table.Column<double>(type: "REAL", nullable: false),
                    Loose3RawValue = table.Column<double>(type: "REAL", nullable: false),
                    Factory1ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factory1Press1ShiftData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factory1Press1ShiftData_Factory1ProductTypes_Factory1ProductTypeId",
                        column: x => x.Factory1ProductTypeId,
                        principalTable: "Factory1ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1Press1ShiftData_Factory1Shifts_Factory1ShiftId",
                        column: x => x.Factory1ShiftId,
                        principalTable: "Factory1Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1Press1ShiftData_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factory1Warehouse1ShiftData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tank1LooseRawValue = table.Column<double>(type: "REAL", nullable: false),
                    Tank2LooseRawValue = table.Column<double>(type: "REAL", nullable: false),
                    Tank3LooseRawValue = table.Column<double>(type: "REAL", nullable: false),
                    Factory1ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factory1Warehouse1ShiftData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factory1Warehouse1ShiftData_Factory1Shifts_Factory1ShiftId",
                        column: x => x.Factory1ShiftId,
                        principalTable: "Factory1Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1Warehouse1ShiftData_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factory1Warehouse2ShiftData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Place1ProductTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Place1ProductsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Place2ProductTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Place2ProductsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Place3ProductTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Place3ProductsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Factory1ShiftId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factory1Warehouse2ShiftData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factory1Warehouse2ShiftData_Factory1ProductTypes_Place1ProductTypeId",
                        column: x => x.Place1ProductTypeId,
                        principalTable: "Factory1ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1Warehouse2ShiftData_Factory1ProductTypes_Place2ProductTypeId",
                        column: x => x.Place2ProductTypeId,
                        principalTable: "Factory1ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1Warehouse2ShiftData_Factory1ProductTypes_Place3ProductTypeId",
                        column: x => x.Place3ProductTypeId,
                        principalTable: "Factory1ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1Warehouse2ShiftData_Factory1Shifts_Factory1ShiftId",
                        column: x => x.Factory1ShiftId,
                        principalTable: "Factory1Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Factory1Warehouse2ShiftData_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Autoclave1ShiftDatas_Factory1ProductTypeId",
                table: "Factory1Autoclave1ShiftDatas",
                column: "Factory1ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Autoclave1ShiftDatas_Factory1ShiftId",
                table: "Factory1Autoclave1ShiftDatas",
                column: "Factory1ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Autoclave1ShiftDatas_ProfileId",
                table: "Factory1Autoclave1ShiftDatas",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1GeneralShiftData_Factory1PackProductTypeId",
                table: "Factory1GeneralShiftData",
                column: "Factory1PackProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1GeneralShiftData_Factory1ProductTypeId",
                table: "Factory1GeneralShiftData",
                column: "Factory1ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1GeneralShiftData_Factory1ShiftId",
                table: "Factory1GeneralShiftData",
                column: "Factory1ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1GeneralShiftData_ProfileId",
                table: "Factory1GeneralShiftData",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Pack1ShiftDatas_Factory1ProductTypeId",
                table: "Factory1Pack1ShiftDatas",
                column: "Factory1ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Pack1ShiftDatas_Factory1ShiftId",
                table: "Factory1Pack1ShiftDatas",
                column: "Factory1ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Pack1ShiftDatas_ProfileId",
                table: "Factory1Pack1ShiftDatas",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Press1ShiftData_Factory1ProductTypeId",
                table: "Factory1Press1ShiftData",
                column: "Factory1ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Press1ShiftData_Factory1ShiftId",
                table: "Factory1Press1ShiftData",
                column: "Factory1ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Press1ShiftData_ProfileId",
                table: "Factory1Press1ShiftData",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Warehouse1ShiftData_Factory1ShiftId",
                table: "Factory1Warehouse1ShiftData",
                column: "Factory1ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Warehouse1ShiftData_ProfileId",
                table: "Factory1Warehouse1ShiftData",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Warehouse2ShiftData_Factory1ShiftId",
                table: "Factory1Warehouse2ShiftData",
                column: "Factory1ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Warehouse2ShiftData_Place1ProductTypeId",
                table: "Factory1Warehouse2ShiftData",
                column: "Place1ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Warehouse2ShiftData_Place2ProductTypeId",
                table: "Factory1Warehouse2ShiftData",
                column: "Place2ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Warehouse2ShiftData_Place3ProductTypeId",
                table: "Factory1Warehouse2ShiftData",
                column: "Place3ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factory1Warehouse2ShiftData_ProfileId",
                table: "Factory1Warehouse2ShiftData",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Factory1Autoclave1ShiftDatas");

            migrationBuilder.DropTable(
                name: "Factory1GeneralShiftData");

            migrationBuilder.DropTable(
                name: "Factory1Pack1ShiftDatas");

            migrationBuilder.DropTable(
                name: "Factory1Press1ShiftData");

            migrationBuilder.DropTable(
                name: "Factory1Warehouse1ShiftData");

            migrationBuilder.DropTable(
                name: "Factory1Warehouse2ShiftData");

            migrationBuilder.DropTable(
                name: "Factory1ProductTypes");

            migrationBuilder.DropTable(
                name: "Factory1Shifts");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
