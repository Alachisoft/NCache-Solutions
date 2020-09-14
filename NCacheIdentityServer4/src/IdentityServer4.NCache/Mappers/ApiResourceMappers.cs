using AutoMapper;
using IdentityServer4.NCache.Entities;

namespace IdentityServer4.NCache.Mappers
{
    public static class ApiResourceMappers
    {
        static ApiResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }
        public static Models.ApiResource ToModel(this ApiResource entity)
        {
            return entity == null ? null : Mapper.Map<Models.ApiResource>(entity);
        }
        public static ApiResource ToEntity(this Models.ApiResource model)
        {
            return model == null ? null : Mapper.Map<ApiResource>(model);
        }
    }
}
