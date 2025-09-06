using AutoMapper;
using bookingEvent.Data;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Services
{
    public class CategoryServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CategoryServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Category
                                 .AsNoTracking()
                                 .ToListAsync();
        }
    }
}
