using AutoMapper;
using IdentityServer4.NCache.Entities;

namespace IdentityServer4.NCache.Mappers
{
    public static class IdentityResourceMappers
    {
        static IdentityResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }
        public static Models.IdentityResource ToModel(this IdentityResource entity)
        {
            return entity == null ? null : Mapper.Map<Models.IdentityResource>(entity);
        }
        public static IdentityResource ToEntity(this Models.IdentityResource model)
        {
            return model == null ? null : Mapper.Map<IdentityResource>(model);
        }
    }
}
