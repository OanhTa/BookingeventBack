namespace bookingEvent.DTO
{
    public class PermissionDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsGranted { get; set; }
        public bool? FromRole { get; set; }

    }
}
