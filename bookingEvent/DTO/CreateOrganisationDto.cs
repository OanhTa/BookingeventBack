namespace bookingEvent.DTO
{
    public class CreateOrganisationDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Logo { get; set; }
    }
    public class InviteUserDto
    {
        public string email { get; set; } 
        public Guid orgId { get; set; }
        public OrganisationUserRole RoleInOrg { get; set; } = OrganisationUserRole.Staff;
    }
}
