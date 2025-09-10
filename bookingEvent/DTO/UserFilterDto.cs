namespace bookingEvent.DTO
{
    public class UserFilterDto
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public DateTime? EmailConfirmedFrom { get; set; }
        public DateTime? EmailConfirmedTo { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? LockoutEnabled { get; set; }
        public bool? IsActive { get; set; }   
        public Guid? RoleId { get; set; }
    }
}
