using AutoMapper;
using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace bookingEvent.Services
{
    public class RoleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public RoleService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .ToListAsync();

            var roleDtos = _mapper.Map<List<RoleDto>>(roles);

            // tính số lượng user
            foreach (var roleDto in roleDtos)
            {
                var role = roles.First(r => r.Id == roleDto.Id);
                roleDto.UserCount = role.UserRoles.Count;
            }

            return roleDtos;
        }

        public async Task<Role?> GetRoleByIdAsync(Guid id)
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            role.Id = Guid.NewGuid();
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> UpdateRoleAsync(Role role)
        {
            var existing = await _context.Roles.FindAsync(role.Id);
            if (existing == null) return false;

            existing.Name = role.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignPermissionsToRoleAsync(Guid roleId, List<Guid> permissionIds)
        {
            var role = await _context.Roles.Include(r => r.RolePermissions)
                                           .FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null) return false;

            // Xóa quyền cũ
            _context.RolePermissions.RemoveRange(role.RolePermissions);

            // Thêm quyền mới
            role.RolePermissions = permissionIds.Select(pid => new RolePermission
            {
                RoleId = roleId,
                PermissionId = pid
            }).ToList();

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
