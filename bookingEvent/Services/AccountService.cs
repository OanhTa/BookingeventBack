using AutoMapper;
using bookingEvent.Data;
using bookingEvent.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace bookingEvent.Services
{
    public class AccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AccountService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> UpdateToken(Guid accoundId, String token)
        {
            var account = await _context.Account.FirstOrDefaultAsync(a => a.Id == accoundId);
            if (account == null)
            {
                return false;
            }
            account.RefreshToken = token;
            account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
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

        public async Task<AccountDTO?> GetAccountDtoAsync(Guid accountId)
        {
            var account = await _context.Account
                .Include(a => a.AccountGroup) // load navigation property
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null) return null;

            return _mapper.Map<AccountDTO>(account);
        }
    } 
}
