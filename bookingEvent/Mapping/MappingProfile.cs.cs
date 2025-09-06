using AutoMapper;
using bookingEvent.DTO;
using bookingEvent.Model;

namespace bookingEvent.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Permission, PermissionDto>();
            CreateMap<UserDto, User>();
            CreateMap<Role, RoleDto>();
            CreateMap<RolePermission, RolePermissionDto>()
                .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission.Name));
            CreateMap<CreateOrganisationDto, Organisation>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.OwnerId, opt => opt.Ignore());
        }

    }
}
