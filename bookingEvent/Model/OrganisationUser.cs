namespace bookingEvent.Model
{
    public class OrganisationUser : BaseEntity
    {
        public Guid OrganisationId { get; set; }
        public Guid UserId { get; set; }

        // RoleInOrg: Owner, Manager, Staff
        public string RoleInOrg { get; set; } = null!;

        // Navigation
        public Organisation Organisation { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
