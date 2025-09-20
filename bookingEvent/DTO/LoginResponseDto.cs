using bookingEvent.Model;

namespace bookingEvent.DTO
{
    public class LoginResponseDto
    {
       public string Token { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public DateTime Expiry { get; set; }
        public List<string> Roles { get; set; }
        public List<OrganisationUser>? OrganisationUsers { get; set; }
    }
}
