using bookingEvent.Data;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Services
{
    public class AccountGroupPermissionServices
    {
        private readonly ApplicationDbContext _context;
        public AccountGroupPermissionServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddPermissionsAsync(List<AccountGroupPermissions> permissions)
        {
            try
            {
                await _context.AccountGroupPermissions.AddRangeAsync(permissions);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<AccountGroupPermissions>> GetPermissionsByGroupAsync(Guid accountGroupId)
        {
            return await _context.AccountGroupPermissions.Where(p => p.AccountGroupId == accountGroupId)
                                 .ToListAsync();
        }

    }
}
