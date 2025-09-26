namespace bookingEvent.Model
{
    public class OrganisationUser : BaseEntity
    {
        public Guid OrganisationId { get; set; }
        public Guid UserId { get; set; }
        public OrganisationUserRole RoleInOrg { get; set; } = OrganisationUserRole.Staff;
        public OrganisationUserStatus Status { get; set; } = OrganisationUserStatus.Pending;
        public Organisation Organisation { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}

public enum OrganisationUserRole
{
    Owner = 0,   // Chủ sở hữu tổ chức
    Manager = 1, // Quản lý
    Staff = 2    // Nhân viên
}

public enum OrganisationUserStatus
{
    Pending = 0,   // Đang chờ duyệt
    Active = 1,    // Đã xác nhận / tham gia
    Cancelled = 2, // Hủy bởi user hoặc tổ chức
    Blocked = 3    // Bị khóa / chặn
}
