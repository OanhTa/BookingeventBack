using bookingEvent.DTO;
using bookingEvent.Model;
using Microsoft.AspNetCore.Mvc;
namespace bookingEvent.Repositories
{
    public interface IPermissionRepository
    {
        Task<List<PermissionDto>> GetRolePermissionsAsync(Guid roleId);
        Task<List<PermissionDto>> GetUserPermissionsAsync(Guid userId);
        Task GrantUserPermissionsAsync(Guid userId, List<string> permissionNames);
        Task RevokeUserPermissionsAsync(Guid userId, List<string> permissionNames);
        Task GrantRolePermissionsAsync(Guid roleId, List<string> permissionNames);
        Task RevokeRolePermissionsAsync(Guid roleId, List<string> permissionNames);

        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
        Task<Permission?> GetPermissionByIdAsync(Guid id);
        Task<Permission> CreatePermissionAsync(Permission permission);
        Task<bool> UpdatePermissionAsync(Permission permission);
        Task<bool> DeletePermissionAsync(Guid id);
    }
}
