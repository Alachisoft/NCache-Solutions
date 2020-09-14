using AutoMapper;
using IdentityServer4.NCache.Entities;
using System.Collections.Generic;

namespace IdentityServer4.NCache.Mappers
{
    internal class IdentityResourceMapperProfile : Profile
    {
        public IdentityResourceMapperProfile()
        {
            CreateMap<IdentityResourceProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<IdentityResource, Models.IdentityResource>(MemberList.Destination)
                .ConstructUsing(src => new Models.IdentityResource())
                .ReverseMap();

            CreateMap<IdentityClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
        }
    }
}
