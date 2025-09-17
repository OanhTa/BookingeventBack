using AutoMapper;
using bookingEvent.Data;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Services
{
    public class AppSettingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AppSettingService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Dictionary<string, string>> GetSettingsByPrefixAsync(string prefix)
        {
            var settings = await _context.AppSettings
                .Where(s => s.Name.StartsWith(prefix))
                .ToListAsync();

            return settings.ToDictionary(
                s => s.Name.Replace(prefix + ".", ""),
                s => s.Value
            );
        }

        public async Task<string?> GetValueAsync(string name, string? providerName = null, string? providerKey = null)
        {
            var setting = await _context.AppSettings
                .FirstOrDefaultAsync(x => x.Name == name);

            return setting?.Value;
        }

        public async Task<bool> IsTrueAsync(string name, string? providerName = null, string? providerKey = null)
        {
            var value = await GetValueAsync(name, providerName, providerKey);
            return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
        }

        public async Task SetValueAsync(string name, string value, string? providerName = null, string? providerKey = null)
        {
            var setting = await _context.AppSettings
                .FirstOrDefaultAsync(x =>
                    x.Name == name &&
                    x.ProviderName == providerName &&
                    x.ProviderKey == providerKey);

            if (setting == null)
            {
                setting = new AppSettings
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Value = value,
                    ProviderName = providerName,
                    ProviderKey = providerKey
                };
                _context.AppSettings.Add(setting);
            }
            else
            {
                setting.Value = value;
            }

            await _context.SaveChangesAsync();
        }
    }
}
