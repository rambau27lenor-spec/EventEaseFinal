using EventEaseFinal.Data;
using EventEaseFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEaseFinal.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.Include(e => e.EventType).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event events)
        {
            if (ModelState.IsValid)
            {
                _context.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", events.EventTypeId);
            return View(events);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var events = await _context.Events.FindAsync(id);
            if (events == null) return NotFound();

            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", events.EventTypeId);
            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event events)
        {
            if (id != events.EventId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", events.EventTypeId);
            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var events = await _context.Events
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (events == null) return NotFound();

            return View(events);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var events = await _context.Events
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (events == null) return NotFound();

            return View(events);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool hasBookings = await _context.Bookings.AnyAsync(b => b.EventId == id);

            if (hasBookings)
            {
                TempData["ErrorMessage"] = "This event cannot be deleted because it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            var events = await _context.Events.FindAsync(id);

            if (events != null)
            {
                _context.Events.Remove(events);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}