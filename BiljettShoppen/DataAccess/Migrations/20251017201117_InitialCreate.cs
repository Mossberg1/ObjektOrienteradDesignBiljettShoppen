using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Arenas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "integer", nullable: false),
                    NumberOfLoges = table.Column<int>(type: "integer", nullable: false),
                    Indoors = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arenas", x => x.Id);
                    table.CheckConstraint("CHK_Arena_NumberOfLoges_NotNegative", "\"NumberOfLoges\" >= 0");
                    table.CheckConstraint("CHK_Arena_NumberOfSeats_NotNegative", "\"NumberOfSeats\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entrances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    VipEntrance = table.Column<bool>(type: "boolean", nullable: false),
                    ArenaId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entrances_Arenas_ArenaId",
                        column: x => x.ArenaId,
                        principalTable: "Arenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    ReleaseTicketsDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NumberOfSeatsToSell = table.Column<int>(type: "integer", nullable: false),
                    NumberOfLogesToSell = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsFamilyFriendly = table.Column<bool>(type: "boolean", nullable: false),
                    ArenaId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.CheckConstraint("CHK_Event_Cost_NotNegative", "\"Cost\" >= 0");
                    table.CheckConstraint("CHK_Event_Date_InFuture", "\"Date\" > CURRENT_TIMESTAMP");
                    table.CheckConstraint("CHK_Event_EndTime_AfterStartTime", "\"EndTime\" > \"StartTime\"");
                    table.CheckConstraint("CHK_Event_HasSomethingToSell", "\"NumberOfSeatsToSell\" > 0 OR \"NumberOfLogesToSell\" > 0");
                    table.CheckConstraint("CHK_Event_NumberOfLogesToSell_NotNegative", "\"NumberOfLogesToSell\" >= 0");
                    table.CheckConstraint("CHK_Event_NumberOfSeatsToSell_NotNegative", "\"NumberOfSeatsToSell\" >= 0");
                    table.CheckConstraint("CHK_Event_Price_NotNegative", "\"Price\" >= 0");
                    table.CheckConstraint("CHK_Event_ReleaseTicketsDate_BeforeEventDate", "\"ReleaseTicketsDate\" < \"Date\"");
                    table.ForeignKey(
                        name: "FK_Events_Arenas_ArenaId",
                        column: x => x.ArenaId,
                        principalTable: "Arenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Method = table.Column<int>(type: "integer", nullable: false),
                    BookingId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookableSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    ArenaId = table.Column<int>(type: "integer", nullable: false),
                    EntranceId = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    LogeNumber = table.Column<string>(type: "text", nullable: true),
                    NumberOfPeople = table.Column<int>(type: "integer", nullable: true),
                    RowNumber = table.Column<int>(type: "integer", nullable: true),
                    ColNumber = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookableSpaces", x => x.Id);
                    table.CheckConstraint("CHK_BookableSpace_Price_NotNegative", "\"Price\" >= 0");
                    table.CheckConstraint("CHK_Loge_NumberOfPeople_NotNegative", "\"NumberOfPeople\" > 0");
                    table.CheckConstraint("CHK_Seat_ColNumber_NotNegative", "\"ColNumber\" >= 0");
                    table.CheckConstraint("CHK_Seat_RowNumber_NotNegative", "\"RowNumber\" >= 0");
                    table.ForeignKey(
                        name: "FK_BookableSpaces_Arenas_ArenaId",
                        column: x => x.ArenaId,
                        principalTable: "Arenas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookableSpaces_Entrances_EntranceId",
                        column: x => x.EntranceId,
                        principalTable: "Entrances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    BookingId = table.Column<int>(type: "integer", nullable: true),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    BookableSpaceId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.CheckConstraint("CHK_Ticket_Price_NotNegative", "\"Price\" >= 0");
                    table.ForeignKey(
                        name: "FK_Tickets_BookableSpaces_BookableSpaceId",
                        column: x => x.BookableSpaceId,
                        principalTable: "BookableSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookableSpaces_ArenaId",
                table: "BookableSpaces",
                column: "ArenaId");

            migrationBuilder.CreateIndex(
                name: "IX_BookableSpaces_EntranceId",
                table: "BookableSpaces",
                column: "EntranceId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_ArenaId",
                table: "Entrances",
                column: "ArenaId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ArenaId",
                table: "Events",
                column: "ArenaId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_BookableSpaceId",
                table: "Tickets",
                column: "BookableSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_BookingId",
                table: "Tickets",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId_BookableSpaceId",
                table: "Tickets",
                columns: new[] { "EventId", "BookableSpaceId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "BookableSpaces");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Entrances");

            migrationBuilder.DropTable(
                name: "Arenas");
        }
    }
}
