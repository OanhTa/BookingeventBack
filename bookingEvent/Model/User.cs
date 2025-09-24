using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? AvatarUrl { get; set; } = null!;
        public bool EmailConfirmed { get; set; } = false;
        public string? EmailConfirmedToken { get; set; }
        public DateTime? EmailConfirmedAt { get; set; }

        public string PasswordHash { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? AccessToken { get; set; }
        public DateTime? TokenExpireTime { get; set; }

        public bool? LockoutEnabled { get; set; } = true;
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; } = 0;

        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }
        public bool? isDelete { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LoginAt { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; } = new List<UserRole>();
        public ICollection<UserPermission>? UserPermissions { get; set; } = new List<UserPermission>();
        public ICollection<OrganisationUser>? OrganisationUsers { get; set; } = new List<OrganisationUser>();
    }
}
