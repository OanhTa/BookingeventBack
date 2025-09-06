namespace bookingEvent.DTO
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<RolePermissionDto> RolePermissions { get; set; }

        public int UserCount { get; set; }
    }
}
