using AutoMapper;
using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace bookingEvent.Services
{
    public class PermissionService : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public PermissionService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<PermissionDto>> GetRolePermissionsAsync(Guid roleId)
        {
            var rolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => new PermissionDto
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name,
                    Description = rp.Permission.Description,
                    IsGranted = rp.IsGranted
                })
                .ToListAsync();

            return rolePermissions;
        }

        public async Task<List<PermissionDto>> GetUserPermissionsAsync(Guid userId)
        {
            // Lấy quyền từ role
            var rolePermissions = await _context.Roles
                .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
                .SelectMany(r => r.RolePermissions)
                .Select(rp => new PermissionDto
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name,
                    Description = rp.Permission.Description,
                    IsGranted = rp.IsGranted
                })
                .ToListAsync();

            // Lấy quyền gán trực tiếp cho user
            var userPermissions = await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => new PermissionDto
                {
                    Id = up.Permission.Id,
                    Name = up.Permission.Name,
                    Description = up.Permission.Description,
                    IsGranted = up.IsGranted
                })
                .ToListAsync();

            // Hợp nhất 2 list, loại trùng theo Id
            var allPermissions = rolePermissions
                .Concat(userPermissions)
                .GroupBy(p => p.Id)
                .Select(g => g.First()) // nếu trùng thì lấy 1 cái
                .ToList();

            return allPermissions;
        }

        public async Task GrantUserPermissionsAsync(Guid userId, List<string> permissionNames)
        {
            // Lấy tất cả quyền trong DB theo danh sách truyền vào
            var permissions = await _context.Permissions
                .Where(p => permissionNames.Contains(p.Name))
                .ToListAsync();

            foreach (var permission in permissions)
            {
                var existing = await _context.UserPermissions
                    .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permission.Id);

                if (existing != null)
                {
                    existing.IsGranted = true;
                }
                else
                {
                    _context.UserPermissions.Add(new UserPermission
                    {
                        UserId = userId,
                        PermissionId = permission.Id,
                        IsGranted = true
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

      
    
        public async Task RevokeUserPermissionsAsync(Guid userId, List<string> permissionNames)
         {
                var permissions = await _context.Permissions
                    .Where(p => permissionNames.Contains(p.Name))
                    .ToListAsync();

                foreach (var permission in permissions)
                {
                    var existing = await _context.UserPermissions
                        .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permission.Id);

                    if (existing != null)
                    {
                        existing.IsGranted = false;
                    }
                }

                await _context.SaveChangesAsync();
        }

        public async Task GrantRolePermissionsAsync(Guid roleId, List<string> permissionNames)
        {
            var permissions = await _context.Permissions
                .Where(p => permissionNames.Contains(p.Name))
                .ToListAsync();

            foreach (var permission in permissions)
            {
                var existing = await _context.RolePermissions
                    .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id);

                if (existing != null)
                {
                    existing.IsGranted = true;
                }
                else
                {
                    _context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permission.Id,
                        IsGranted = true
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RevokeRolePermissionsAsync(Guid roleId, List<string> permissionNames)
        {
            var permissions = await _context.Permissions
                .Where(p => permissionNames.Contains(p.Name))
                .ToListAsync();

            foreach (var permission in permissions)
            {
                var existing = await _context.RolePermissions
                    .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id);

                if (existing != null)
                {
                    existing.IsGranted = false; 
                }
            }

            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<Permission?> GetPermissionByIdAsync(Guid id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public async Task<Permission> CreatePermissionAsync(Permission permission)
        {
            permission.Id = Guid.NewGuid();
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
            return permission;
        }

        public async Task<bool> UpdatePermissionAsync(Permission permission)
        {
            var existing = await _context.Permissions.FindAsync(permission.Id);
            if (existing == null) return false;

            existing.Name = permission.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePermissionAsync(Guid id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return false;

            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
