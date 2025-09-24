using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class Organisation : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }  // cho phép null
        public string? Logo { get; set; }

        public Guid OwnerId { get; set; }   // FK tới User.Id

        // Navigation
        public User Owner { get; set; } = null!;
        public ICollection<OrganisationUser> OrganisationUsers { get; set; } = new List<OrganisationUser>();
        public ICollection<Event> Events { get; set; } = new List<Event>();

    }
}
