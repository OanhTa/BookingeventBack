namespace bookingEvent.DTO
{
    public class UserPermissionDto
    {
        public string Name { get; set; } = string.Empty;   
        public string Description { get; set; } = string.Empty;
        public bool IsGranted { get; set; }
    }
}
