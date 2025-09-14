using bookingEvent.DTO;
using bookingEvent.Model;

namespace bookingEvent.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(Guid id);
        Task<IEnumerable<Role>> SearchRolesAsync(string keyword);
        Task<int> MoveUsersToRoleAsync(Guid oldRoleId, Guid newRoleId);
        Task<Role> CreateRoleAsync(Role role);
        Task<bool> UpdateRoleAsync(Role role);
        Task<bool> DeleteRoleAsync(Guid id);
        Task<bool> AssignPermissionsToRoleAsync(Guid roleId, List<Guid> permissionIds);
    }
}
