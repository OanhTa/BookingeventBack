using AutoMapper;
using bookingEvent.DTO;
using bookingEvent.Model;

namespace bookingEvent.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Account, AccountDTO>();
            CreateMap<AccountGroup, AccountGroupDTO>();
        }

    }
}
