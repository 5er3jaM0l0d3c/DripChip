using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:AnimalGender", "MALE,FEMALE,OTHER")
                .Annotation("Npgsql:Enum:AnimalLifeStatus", "ALIVE,DEAD");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Account_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimalType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AnimalType_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Location_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Animal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Length = table.Column<float>(type: "real", nullable: false),
                    Height = table.Column<float>(type: "real", nullable: false),
                    ChipperId = table.Column<int>(type: "integer", nullable: false),
                    ChippingLocationId = table.Column<long>(type: "bigint", nullable: false),
                    ChippingDateTime = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: true, defaultValueSql: "now()"),
                    DeathDateTime = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: true),
                    LifeStatus = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Animal_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animal_Account_ChipperId",
                        column: x => x.ChipperId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Animal_Location_ChippingLocationId",
                        column: x => x.ChippingLocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animal_AnimalType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimalTypeId = table.Column<long>(type: "bigint", nullable: false),
                    AnimalId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Animal_AnimalType_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animal_AnimalType_AnimalType_AnimalTypeId",
                        column: x => x.AnimalTypeId,
                        principalTable: "AnimalType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Animal_AnimalType_Animal_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animal_Location",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationId = table.Column<long>(type: "bigint", nullable: false),
                    AnimalId = table.Column<long>(type: "bigint", nullable: false),
                    DateTimeOfVisitLocationPoint = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Animal_Location_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animal_Location_Animal_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Animal_Location_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animal_ChipperId",
                table: "Animal",
                column: "ChipperId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_ChippingLocationId",
                table: "Animal",
                column: "ChippingLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_AnimalType_AnimalId",
                table: "Animal_AnimalType",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_AnimalType_AnimalTypeId",
                table: "Animal_AnimalType",
                column: "AnimalTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_Location_AnimalId",
                table: "Animal_Location",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_Location_LocationId",
                table: "Animal_Location",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animal_AnimalType");

            migrationBuilder.DropTable(
                name: "Animal_Location");

            migrationBuilder.DropTable(
                name: "AnimalType");

            migrationBuilder.DropTable(
                name: "Animal");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Location");
        }
    }
}
