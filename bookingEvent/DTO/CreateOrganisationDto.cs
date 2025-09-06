namespace bookingEvent.DTO
{
    public class CreateOrganisationDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Logo { get; set; }
    }
}
