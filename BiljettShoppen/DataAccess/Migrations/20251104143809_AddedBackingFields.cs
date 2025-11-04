using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedBackingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfEntrances",
                table: "Arenas");

            migrationBuilder.DropColumn(
                name: "NumberOfLoges",
                table: "Arenas");

            migrationBuilder.DropColumn(
                name: "NumberOfSeats",
                table: "Arenas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfEntrances",
                table: "Arenas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfLoges",
                table: "Arenas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeats",
                table: "Arenas",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
