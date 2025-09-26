using bookingEvent.DTO;
using bookingEvent.Model;
using Microsoft.AspNetCore.Mvc;
namespace bookingEvent.Repositories
{
    public interface IOrganisationRepository
    {
        Task<Organisation> CreateOrganisationAsync(CreateOrganisationDto dto, Guid userId);
        Task<bool> InviteUserAsync(InviteUserDto dto);
        Task<List<OrganisationUser>> GetOrganisationsByUserAsync(Guid userId);
        Task<List<object>> GetUserOrganisationsAsync(Guid userId);
        Task<List<object>> GetUsersByOrganisationAsync(Guid orgId);
        Task<Organisation?> UpdateOrganisationAsync(Guid id, CreateOrganisationDto dto, Guid userId);
        Task<bool> DeleteOrganisationAsync(Guid id, Guid userId);
    }
}
