using bookingEvent.Model;

namespace bookingEvent.DTO
{
    public class CreateUserDto
    {
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public List<Guid>? RoleIds { get; set; } = new();
        public List<Guid>? OrganisationIds { get; set; } = new();
    }

    public class UpdateUserDto
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; } = null!;
        public string? FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? PasswordHash { get; set; }
        public List<Guid>? RoleIds { get; set; } = new();
        public List<Guid>? OrganisationIds { get; set; } = new();
    }


    public class UserDto
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; } = null!;
        public string? Email { get; set; } = null!;

        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? AccessToken { get; set; }
        public DateTime? TokenExpireTime { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; } = new List<UserRole>();
    }

    public class SetPasswordDto
    {
        public Guid Id { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
    }
}
