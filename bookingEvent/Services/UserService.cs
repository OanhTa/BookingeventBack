using AutoMapper;
using bookingEvent.Const;
using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace bookingEvent.Services
{
    public class UserService : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettingService _settingService;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context, IMapper mapper, AppSettingService settingService)
        {
            _context = context;
            _mapper = mapper;
            _settingService = settingService;
        }

        public async Task<User> CreateUserAsync(CreateUserDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName,
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash),
                Phone = dto.Phone,
                Address = dto.Address
            };

            // Gán roles
            user.UserRoles = dto.RoleIds.Select(rid => new UserRole
            {
                UserId = user.Id,   
                RoleId = rid
            }).ToList();

            // Gán organisations
            user.OrganisationUsers = dto.OrganisationIds.Select(oid => new OrganisationUser
            {
                UserId = user.Id,  
                OrganisationId = oid
            }).ToList();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .Include(u => u.OrganisationUsers)
                .FirstOrDefaultAsync(u => u.Id == dto.Id);

            if (user == null) return false;

            user.UserName = dto.UserName;
            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Address = dto.Address;

            if (!string.IsNullOrEmpty(dto.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
            }

            // Xử lý Roles: clear cũ -> add mới
            user.UserRoles.Clear();
            foreach (var roleId in dto.RoleIds)
            {
                user.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId
                });
            }

            user.OrganisationUsers.Clear();
            foreach (var orgId in dto.OrganisationIds)
            {
                user.OrganisationUsers.Add(new OrganisationUser
                {
                    UserId = user.Id,
                    OrganisationId = orgId
                });
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProfileAsync(UpdateUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (user == null) return false;

            user.UserName = dto.UserName;
            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Address = dto.Address;
            
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .ToListAsync();
        }
        public async Task<IEnumerable<User>> SearchUsersAsync(UserFilterDto filter)
        {
            var query = _context.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .AsQueryable();

            if (filter.Id.HasValue)
                query = query.Where(u => u.Id == filter.Id.Value);

            if (!string.IsNullOrEmpty(filter.UserName))
                query = query.Where(u => u.UserName.Contains(filter.UserName));

            if (!string.IsNullOrEmpty(filter.FullName))
                query = query.Where(u => u.FullName!.Contains(filter.FullName));

            if (filter.CreatedFrom.HasValue)
                query = query.Where(u => u.CreatedAt >= filter.CreatedFrom.Value);

            if (filter.CreatedTo.HasValue)
                query = query.Where(u => u.CreatedAt <= filter.CreatedTo.Value);

            if (filter.EmailConfirmedFrom.HasValue)
                query = query.Where(u => u.EmailConfirmedAt >= filter.EmailConfirmedFrom.Value);

            if (filter.EmailConfirmedTo.HasValue)
                query = query.Where(u => u.EmailConfirmedAt <= filter.EmailConfirmedTo.Value);

            if (!string.IsNullOrEmpty(filter.Phone))
                query = query.Where(u => u.Phone!.Contains(filter.Phone));

            if (!string.IsNullOrEmpty(filter.Email))
                query = query.Where(u => u.Email.Contains(filter.Email));

            if (filter.EmailConfirmed.HasValue)
                query = query.Where(u => u.EmailConfirmed == filter.EmailConfirmed);

            if (filter.LockoutEnabled.HasValue)
                query = query.Where(u => u.LockoutEnabled == filter.LockoutEnabled);

            if (filter.IsActive.HasValue)
                query = query.Where(u => (u.LockoutEnd == null || u.LockoutEnd < DateTimeOffset.UtcNow) == filter.IsActive);

            if (filter.RoleId.HasValue)
                query = query.Where(u => u.UserRoles.Any(ur => ur.RoleId == filter.RoleId.Value));

            return await query.ToListAsync();
        }
        public async Task<List<User>> SearchUsersAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<User>();

            var query = _context.Users.AsQueryable();

            query = query.Where(u =>
                u.UserName.Contains(keyword) ||
                (u.FullName != null && u.FullName.Contains(keyword)) ||
                (u.Email != null && u.Email.Contains(keyword)) ||
                (u.Phone != null && u.Phone.Contains(keyword))
            );

            return await query.ToListAsync();
        }
        public async Task<bool> IsTokenValidAsync(Guid userId, string token)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId && u.AccessToken == token);

            if (user == null) return false;
            if (user.TokenExpireTime == null) return false;

            return user.TokenExpireTime > DateTime.UtcNow;
        }
        public async Task<bool> SetPasswordAsync(SetPasswordDto dto)
        {
            var user = await _context.Users.FindAsync(dto.Id);
            if (user == null) return false;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            // Chỉ update field PasswordHash, không đụng các field khác
            _context.Entry(user).Property(u => u.PasswordHash).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            if (!_context.UserRoles.Any(ur => ur.UserId == userId && ur.RoleId == roleId))
            {
                _context.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToListAsync();
        }
        public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission)
                .Distinct()
                .ToListAsync();
        }
        public async Task<bool> LockUserAsync(Guid userId, DateTimeOffset lockEnd)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            user.LockoutEnabled = true;
            user.LockoutEnd = lockEnd;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UnlockUserAsync(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            user.LockoutEnd = null;
            user.LockoutEnabled = false;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
