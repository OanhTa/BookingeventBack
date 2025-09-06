using AutoMapper;
using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace bookingEvent.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
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

        public async Task<bool> IsTokenValidAsync(Guid userId, string token)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId && u.AccessToken == token);

            if (user == null) return false;
            if (user.TokenExpireTime == null) return false;

            return user.TokenExpireTime > DateTime.UtcNow;
        }

        public async Task<bool> UpdateUserAsync(UserDto dto)
        {
            var user = await _context.Users.FindAsync(dto.Id);
            if (user == null) return false;

            _mapper.Map(dto, user);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetPasswordAsync(SetPasswordDto dto)
        {
            var user = await _context.Users.FindAsync(dto.Id);
            if (user == null) return false;

            // Hash mật khẩu mới bằng BCrypt
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
    }
}
