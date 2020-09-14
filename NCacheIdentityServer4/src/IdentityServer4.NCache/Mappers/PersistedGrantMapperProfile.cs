using AutoMapper;
using IdentityServer4.NCache.Entities;

namespace IdentityServer4.NCache.Mappers
{
    internal class PersistedGrantMapperProfile : Profile
    {
        public PersistedGrantMapperProfile()
        {
            CreateMap<PersistedGrant, Models.PersistedGrant>(MemberList.Destination)
                .ReverseMap();
        }
    }
}
