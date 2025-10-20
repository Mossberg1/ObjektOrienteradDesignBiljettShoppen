using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeatLayout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookableSpaces_Arenas_ArenaId",
                table: "BookableSpaces");

            migrationBuilder.DropIndex(
                name: "IX_BookableSpaces_ArenaId",
                table: "BookableSpaces");

            migrationBuilder.DropColumn(
                name: "ArenaId",
                table: "BookableSpaces");

            migrationBuilder.AddColumn<int>(
                name: "SeatLayoutId",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeatLayoutId",
                table: "BookableSpaces",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SeatConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumberOfRows = table.Column<int>(type: "integer", nullable: false),
                    NumberOfCols = table.Column<int>(type: "integer", nullable: false),
                    ArenaId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeatConfigurations_Arenas_ArenaId",
                        column: x => x.ArenaId,
                        principalTable: "Arenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_SeatLayoutId",
                table: "Events",
                column: "SeatLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_BookableSpaces_SeatLayoutId",
                table: "BookableSpaces",
                column: "SeatLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatConfigurations_ArenaId",
                table: "SeatConfigurations",
                column: "ArenaId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookableSpaces_SeatConfigurations_SeatLayoutId",
                table: "BookableSpaces",
                column: "SeatLayoutId",
                principalTable: "SeatConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_SeatConfigurations_SeatLayoutId",
                table: "Events",
                column: "SeatLayoutId",
                principalTable: "SeatConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookableSpaces_SeatConfigurations_SeatLayoutId",
                table: "BookableSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_SeatConfigurations_SeatLayoutId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "SeatConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_Events_SeatLayoutId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_BookableSpaces_SeatLayoutId",
                table: "BookableSpaces");

            migrationBuilder.DropColumn(
                name: "SeatLayoutId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SeatLayoutId",
                table: "BookableSpaces");

            migrationBuilder.AddColumn<int>(
                name: "ArenaId",
                table: "BookableSpaces",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookableSpaces_ArenaId",
                table: "BookableSpaces",
                column: "ArenaId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookableSpaces_Arenas_ArenaId",
                table: "BookableSpaces",
                column: "ArenaId",
                principalTable: "Arenas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
