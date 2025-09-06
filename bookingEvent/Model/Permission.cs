namespace bookingEvent.Model
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!; // vd: "Identity.Users.Create"
        public string? Description { get; set; }
    }
}
