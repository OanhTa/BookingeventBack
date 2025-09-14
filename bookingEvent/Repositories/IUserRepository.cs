using bookingEvent.DTO;
using bookingEvent.Model;

namespace bookingEvent.Repositories
{
    public interface IUserRepository 
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(CreateUserDto dto);
        Task<bool> UpdateUserAsync(UpdateUserDto dto);
        Task<bool> UpdateProfileAsync(UpdateUserDto dto);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<User>> SearchUsersAsync(UserFilterDto filter);
        Task<List<User>> SearchUsersAsync(string keyword);
        Task<bool> IsTokenValidAsync(Guid userId, string token);
        Task<bool> SetPasswordAsync(SetPasswordDto dto);
        Task<bool> DeleteUserAsync(Guid id);
        Task AssignRoleToUserAsync(Guid userId, Guid roleId);
        Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId);
        Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId);
        Task<bool> LockUserAsync(Guid userId, DateTimeOffset lockEnd);
        Task<bool> UnlockUserAsync(Guid userId);
    }
}
