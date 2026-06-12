using EventEaseFinal.Data;
using EventEaseFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEaseFinal.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? eventTypeId, DateTime? startDate, DateTime? endDate, bool? isAvailable)
        {
            var bookings = _context.Bookings
                .Include(b => b.Event).ThenInclude(e => e.EventType)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b =>
                    b.Event.EventName.Contains(searchString) ||
                    b.Venue.VenueName.Contains(searchString) ||
                    b.BookingId.ToString().Contains(searchString));
            }

            if (eventTypeId.HasValue)
                bookings = bookings.Where(b => b.Event.EventTypeId == eventTypeId.Value);

            if (startDate.HasValue)
                bookings = bookings.Where(b => b.BookingDate >= startDate.Value);

            if (endDate.HasValue)
                bookings = bookings.Where(b => b.BookingDate <= endDate.Value);

            if (isAvailable.HasValue)
                bookings = bookings.Where(b => b.Venue.IsAvailable == isAvailable.Value);

            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventTypeId);
            ViewBag.SearchString = searchString;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(await bookings.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName");
            ViewData["VenueId"] = new SelectList(_context.Venues.Where(v => v.IsAvailable), "VenueId", "VenueName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                bool bookingExists = await _context.Bookings
                    .AnyAsync(b => b.VenueId == booking.VenueId && b.BookingDate == booking.BookingDate);

                if (bookingExists)
                {
                    ModelState.AddModelError("", "This venue is already booked for the selected date.");
                    ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
                    ViewData["VenueId"] = new SelectList(_context.Venues.Where(v => v.IsAvailable), "VenueId", "VenueName", booking.VenueId);
                    return View(booking);
                }

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues.Where(v => v.IsAvailable), "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }
    }
}
