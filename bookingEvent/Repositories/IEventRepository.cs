using bookingEvent.Model;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Repositories
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(Guid id);
        Task<List<Event>> GetEventsByOrganisationAsync(Guid organisationId);
        Task<List<Event>> GetEventsByUserAsync(Guid userId);
        Task<Event> CreateAsync(Event newEvent, EventDetail detail);
        Task<Event?> UpdateAsync(Guid id, Event updatedEvent);
        Task<bool> DeleteAsync(Guid id);
    }
}
