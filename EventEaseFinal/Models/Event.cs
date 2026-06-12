using System.ComponentModel.DataAnnotations;

namespace EventEaseFinal.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public string? Description { get; set; }

        [Required]
        public int EventTypeId { get; set; }

        public EventType? EventType { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}
