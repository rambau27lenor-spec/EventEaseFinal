using System.ComponentModel.DataAnnotations;

namespace EventEaseFinal.Models
{
    public class EventType
    {
        public int EventTypeId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Event>? Events { get; set; }
    }
}
