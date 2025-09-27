using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class Organisation : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Description { get; set; }  
        public string? Logo { get; set; }
        public string? Website { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? LinkedIn { get; set; }
        public string? Youtube { get; set; }
        public string? PostalCode { get; set; }

        public Guid OwnerId { get; set; }   // FK tới User.Id

        // Navigation
        public User Owner { get; set; } = null!;
        public ICollection<OrganisationUser> OrganisationUsers { get; set; } = new List<OrganisationUser>();
        public ICollection<Event> Events { get; set; } = new List<Event>();

    }
}
