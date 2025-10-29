using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedPendingBookingReferenceToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PendingBookingReference",
                table: "Tickets",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ReferenceNumber",
                table: "Bookings",
                column: "ReferenceNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_ReferenceNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PendingBookingReference",
                table: "Tickets");
        }
    }
}
