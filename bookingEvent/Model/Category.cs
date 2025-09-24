using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class Category : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Event>? Events { get; set; }
    }
}
