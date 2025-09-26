using bookingEvent.Model;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Repositories
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(Guid id);
        Task<IEnumerable<Event>> SearchAsync(string keyword);
        Task<List<Event>> GetEventsByOrganisationAsync(Guid organisationId, EventStatus? status);
        Task<List<Event>> GetEventsByUserAsync(Guid userId);
        Task<Event> CreateAsync(Event newEvent, EventDetail detail, ICollection<TicketType> tickets);
        Task<Event?> UpdateAsync(Guid id, Event updatedEvent);
        Task<bool> DeleteAsync(Guid id);
    }
}
