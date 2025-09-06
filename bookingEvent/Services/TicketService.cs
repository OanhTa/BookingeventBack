using AutoMapper;
using bookingEvent.Data;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Services
{
    public class TicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TicketService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TicketType>> GetAllAsync()
        {
            return await _context.TicketType
                .Include(t => t.Event)
                .Include(t => t.Tickets)
                    .ThenInclude(ts => ts.User)
                .ToListAsync();
        }

        public async Task<TicketType?> GetByIdAsync(Guid id)
        {
            return await _context.TicketType
                .Include(t => t.Event)
                .Include(t => t.Tickets)
                    .ThenInclude(ts => ts.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TicketType> CreateAsync(TicketType model)
        {
            model.Id = Guid.NewGuid();
            _context.TicketType.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> UpdateAsync(Guid id, TicketType model)
        {
            var ticket = await _context.TicketType.FindAsync(id);
            if (ticket == null) return false;

            ticket.Name = model.Name;
            ticket.Price = model.Price;
            ticket.Quantity = model.Quantity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var ticket = await _context.TicketType.FindAsync(id);
            if (ticket == null) return false;

            _context.TicketType.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetCustomersAsync(Guid ticketTypeId)
        {
            var ticket = await _context.TicketType
                .Include(t => t.Tickets)
                    .ThenInclude(ts => ts.User)
                .FirstOrDefaultAsync(t => t.Id == ticketTypeId);

            if (ticket == null) return new List<User>();
            return ticket.Tickets.Select(ts => ts.User).ToList();
        }
    }
}
