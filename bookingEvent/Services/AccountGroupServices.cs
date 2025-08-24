using bookingEvent.Data;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Services
{
    public class AccountGroupServices
    {
        private readonly ApplicationDbContext _context;
        public AccountGroupServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAccountGroupAsync(AccountGroup accountgroup)
        {
            try
            {
                await _context.AccountGroup.AddAsync(accountgroup);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<AccountGroup>> GetAccountGroupAsync()
        {
            return await _context.AccountGroup.ToListAsync();
        }

    }
}
