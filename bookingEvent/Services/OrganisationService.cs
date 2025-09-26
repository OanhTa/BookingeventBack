using AutoMapper;
using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace bookingEvent.Services
{
    public class OrganisationService : IOrganisationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly EmailService _emailService;
        private readonly NotificationService _notificationService;

        public OrganisationService(ApplicationDbContext context, IMapper mapper, EmailService emailService, NotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _notificationService = notificationService;
        }

        public static string GetRoleText(OrganisationUserRole role)
        {
            return role switch
            {
                OrganisationUserRole.Owner => "Chủ sở hữu",
                OrganisationUserRole.Manager => "Quản lý",
                OrganisationUserRole.Staff => "Thành viên",
                _ => "Không xác định"
            };
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
                RoleInOrg = OrganisationUserRole.Owner
            });

            await _context.SaveChangesAsync();
            return organisation;
        }

        public async Task<bool> InviteUserAsync(InviteUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.email);
            if (user == null)
                return false;

            var organisation = await _context.Organisation.FirstOrDefaultAsync(o => o.Id == dto.orgId);
            if (organisation == null)
                return false;

            _context.OrganisationUser.Add(new OrganisationUser
            {
                OrganisationId = dto.orgId,
                UserId = user.Id,
                RoleInOrg = dto.RoleInOrg,
                Status = OrganisationUserStatus.Pending,
            });

            var token = Guid.NewGuid().ToString();
            var inviteLink = $"http://localhost:4200/invite-member?token={token}";

            var subject = "Lời mời tham gia tổ chức";
            var body = $@"
                Xin chào {user.FullName},<br/><br/>
                Bạn đã được mời tham gia tổ chức <b>{organisation.Name}</b> với vai trò <b>{GetRoleText(dto.RoleInOrg)}</b>.<br/>
                Vui lòng click vào link sau để chấp nhận lời mời:<br/>
                <a href='{inviteLink}'>Tham gia ngay</a><br/><br/>
                Nếu bạn không muốn tham gia, vui lòng bỏ qua email này.
            ";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            var notification = await _notificationService.CreateNotificationAsync(
                dto.orgId, 
                "Thành viên mới trong tổ chức",
                $"Người dùng {user.FullName} đã được mời tham gia tổ chức với vai trò {GetRoleText(dto.RoleInOrg)}.",
                NotificationType.General
            );

            return await _context.SaveChangesAsync() > 0;
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
                    ou.User.LoginAt,
                    ou.Status,
                    Role = ou.RoleInOrg,
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
