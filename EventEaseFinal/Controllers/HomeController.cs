using EventEaseFinal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEaseFinal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalVenues = await _context.Venues.CountAsync();
            ViewBag.TotalEvents = await _context.Events.CountAsync();
            ViewBag.TotalBookings = await _context.Bookings.CountAsync();

            return View();
        }
    }
}