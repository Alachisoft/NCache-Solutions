using AutoMapper;
using IdentityServer4.NCache.Entities;

namespace IdentityServer4.NCache.Mappers
{
    public static class ClientMappers
    {
        static ClientMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }
        public static Models.Client ToModel(this Client entity)
        {
            return Mapper.Map<Models.Client>(entity);
        }
        public static Client ToEntity(this Models.Client model)
        {
            return Mapper.Map<Client>(model);
        }
    }
}
