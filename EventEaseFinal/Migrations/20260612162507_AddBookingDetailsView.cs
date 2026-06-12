using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEaseFinal.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingDetailsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE VIEW BookingDetailsView AS
SELECT 
    b.BookingId,
    e.EventName,
    et.Name AS EventType,
    v.VenueName,
    v.IsAvailable,
    b.BookingDate
FROM Bookings b
INNER JOIN Events e ON b.EventId = e.EventId
INNER JOIN EventTypes et ON e.EventTypeId = et.EventTypeId
INNER JOIN Venues v ON b.VenueId = v.VenueId
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW BookingDetailsView");
        }
    }
}
