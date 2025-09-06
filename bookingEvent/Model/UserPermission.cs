namespace bookingEvent.Model
{
    public class UserPermission
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;

        public bool IsGranted { get; set; }
    }
}
