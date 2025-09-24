using AutoMapper;
using bookingEvent.Data;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Services
{
    public class EventService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public EventService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<Event>> GetAllAsync()
        {
            return await _context.Event
                .Include(e => e.EventDetail)
                .Include(e => e.TicketTypes)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> SearchAsync(string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetAllAsync();

            keyword = keyword.Trim().ToLower();

            return await _context.Event
                .Include(e => e.EventDetail)
                .Include(e => e.TicketTypes)
                .Where(e => e.Name.ToLower().Contains(keyword)
                         || e.Status.ToString().Contains(keyword))
                .ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            return await _context.Event
                .Include(e => e.EventDetail)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Lấy event theo Organisation
        public async Task<List<Event>> GetEventsByOrganisationAsync(Guid organisationId)
        {
            return await _context.Event
                .Include(e => e.Category)
                .Include(e => e.EventDetail)
                .Include(e => e.TicketTypes)
                .Where(e => e.OrganisationId == organisationId)
                .ToListAsync();
        }

        // Lấy event theo User
        public async Task<List<Event>> GetEventsByUserAsync(Guid userId)
        {
            return await _context.OrganisationUser
                .Where(ou => ou.UserId == userId)
                .SelectMany(ou => ou.Organisation.Events)
                .Include(e => e.Category)
                .Include(e => e.EventDetail)
                .ToListAsync();
        }

        public async Task<Event> CreateAsync(Event newEvent, EventDetail detail, ICollection<TicketType> tickets)
        {
            newEvent.Id = Guid.NewGuid();
            detail.Id = Guid.NewGuid();
            detail.EventId = newEvent.Id;

            await _context.Event.AddAsync(newEvent);
            await _context.EventDetail.AddAsync(detail);
            if (tickets != null && tickets.Any())
            {
                foreach (var ticket in tickets)
                {
                    ticket.Id = Guid.NewGuid();
                    ticket.EventId = newEvent.Id;
                }

                await _context.TicketType.AddRangeAsync(tickets);
            }

            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task<Event?> UpdateAsync(Guid id, Event updatedEvent)
        {
            var existing = await _context.Event.FindAsync(id);
            if (existing == null) return null;

            existing.Name = updatedEvent.Name;
            existing.PriceFrom = updatedEvent.PriceFrom;
            existing.Date = updatedEvent.Date;
            existing.Time = updatedEvent.Time;
            existing.Duration = updatedEvent.Duration;
            existing.Thumbnail = updatedEvent.Thumbnail;
            existing.Status = updatedEvent.Status;
            existing.CategoryId = updatedEvent.CategoryId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var ev = await _context.Event.FindAsync(id);
            if (ev == null) return false;

            _context.Event.Remove(ev);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
