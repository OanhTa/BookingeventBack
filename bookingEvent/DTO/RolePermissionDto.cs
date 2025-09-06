namespace bookingEvent.DTO
{
    public class RolePermissionDto
    {
        public Guid PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool IsGranted { get; set; }
    }
}
