using AutoMapper;
using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Services
{
    public class OrganisationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrganisationService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Organisation> CreateOrganisationAsync(CreateOrganisationDto dto, Guid userId)
        {
            var organisation = _mapper.Map<Organisation>(dto);
            organisation.Id = Guid.NewGuid();
            organisation.OwnerId = userId;

            _context.Organisation.Add(organisation);
            _context.OrganisationUser.Add(new OrganisationUser
            {
                OrganisationId = organisation.Id,
                UserId = userId,
                RoleInOrg = "Owner"
            });

            await _context.SaveChangesAsync();
            return organisation;
        }


        public async Task<List<object>> GetUserOrganisationsAsync(Guid userId)
        {
            return await _context.OrganisationUser
                .Where(ou => ou.UserId == userId)
                .Include(ou => ou.Organisation)
                .Select(ou => new
                {
                    ou.Organisation.Id,
                    ou.Organisation.Name,
                    ou.Organisation.Logo,
                    Role = ou.RoleInOrg
                })
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetUsersByOrganisationAsync(Guid orgId)
        {
            return await _context.OrganisationUser
                .Where(ou => ou.OrganisationId == orgId)
                .Include(ou => ou.User)
                .Select(ou => new
                {
                    ou.User.Id,
                    ou.User.UserName,
                    ou.User.Email,
                    Role = ou.RoleInOrg
                })
                .ToListAsync<object>();
        }

        public async Task<Organisation?> UpdateOrganisationAsync(Guid id, CreateOrganisationDto dto, Guid userId)
        {
            var org = await _context.Organisation.FirstOrDefaultAsync(o => o.Id == id && o.OwnerId == userId);
            if (org == null) return null;

            org.Name = dto.Name;
            org.Description = dto.Description;
            org.Logo = dto.Logo;

            await _context.SaveChangesAsync();
            return org;
        }

        public async Task<bool> DeleteOrganisationAsync(Guid id, Guid userId)
        {
            var org = await _context.Organisation.FirstOrDefaultAsync(o => o.Id == id && o.OwnerId == userId);
            if (org == null) return false;

            _context.Organisation.Remove(org);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrganisationUser>> GetOrganisationsByUserAsync(Guid userId)
        {
            var orgs = await _context.OrganisationUser
                .Include(ou => ou.Organisation)
                .Where(ou => ou.UserId == userId)
                .ToListAsync();

            return orgs;
        }

    }
}
