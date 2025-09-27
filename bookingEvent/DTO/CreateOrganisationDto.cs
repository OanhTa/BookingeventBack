namespace bookingEvent.DTO
{
    public class CreateOrganisationDto
    {
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
    }
    public class InviteUserDto
    {
        public string email { get; set; } 
        public Guid orgId { get; set; }
        public OrganisationUserRole RoleInOrg { get; set; } = OrganisationUserRole.Staff;
    }
}
