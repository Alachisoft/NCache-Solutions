using AutoMapper;
using IdentityServer4.NCache.Entities;

namespace IdentityServer4.NCache.Mappers
{
    public static class PersistedGrantMappers
    {
        static PersistedGrantMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<PersistedGrantMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static Models.PersistedGrant ToModel(this PersistedGrant entity)
        {
            return entity == null ? null : Mapper.Map<Models.PersistedGrant>(entity);
        }

        public static PersistedGrant ToEntity(this Models.PersistedGrant model)
        {
            return model == null ? null : Mapper.Map<PersistedGrant>(model);
        }
        public static void UpdateEntity(this Models.PersistedGrant model, PersistedGrant entity)
        {
            Mapper.Map(model, entity);
        }
    }
}
