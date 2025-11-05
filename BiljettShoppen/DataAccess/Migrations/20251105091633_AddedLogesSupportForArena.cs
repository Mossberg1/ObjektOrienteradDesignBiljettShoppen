using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedLogesSupportForArena : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArenaId",
                table: "BookableSpaces",
                type: "integer",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
