using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class AccountGroup
    {
        [Key]
        public Guid Id { get; set; }
        public String Name { get; set; }
        public bool Enable { get; set; }
        public String Note { get; set; }
    }
}
