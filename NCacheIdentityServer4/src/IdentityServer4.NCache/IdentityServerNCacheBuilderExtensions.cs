using System;
using IdentityServer4.Stores;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using IdentityServer4.NCache.Options;
using IdentityServer4.NCache.Stores;
using IdentityServer4.NCache.Services;
using IdentityServer4.NCache.Caching;
using IdentityServer4.NCache.Stores.Handles;
using IdentityServer4.NCache.Stores.Interfaces;
using IdentityServer4.NCache.Stores.Repositories;
using IdentityServer4.NCache.Entities;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServerNCacheBuilderExtensions
    {
        public static IIdentityServerBuilder AddNCacheConfigurationStore(
            this IIdentityServerBuilder builder,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            builder.Services.AddConfigurationStoreRepository(storeOptionsAction);
            builder.Services.AddSingleton<IClientStore, ClientStore>();
            builder.Services.AddSingleton<IResourceStore,ResourceStore>();
            builder.Services.AddSingleton<ICorsPolicyService,CorsPolicyService>();

            return builder;
        }

        public static IIdentityServerBuilder AddNCachePersistedGrantStore(
            this IIdentityServerBuilder builder,
            Action<PersistedStoreOptions> storeOptionsAction = null)
        {
            builder.Services.AddPersistedStoreRepository(storeOptionsAction);
            builder.Services.AddSingleton<IPersistedGrantStore, 
                                                PersistedGrantStore>();

            return builder;
        }

        public static IIdentityServerBuilder AddNCacheDeviceCodeStore(
            this IIdentityServerBuilder builder,
            Action<DeviceStoreOptions> storeOptionsAction = null)
        {
            builder.Services.AddDeviceStoreRepository(storeOptionsAction);
            builder.Services.AddSingleton<IDeviceFlowStore,
                                                DeviceFlowStore>();

            return builder;
        }

        public static IIdentityServerBuilder AddNCacheCaching(
            this IIdentityServerBuilder builder,
            Action<ICacheOptions> ncacheOptionsAction = null)
        {
            var options = new ICacheOptions();
            ncacheOptionsAction?.Invoke(options);
            var logger = 
                builder
                    .Services
                            .BuildServiceProvider()
                                .GetRequiredService<ILogger<CacheHandle>>();

            builder.Services.AddSingleton<CacheHandle>(
                new CacheHandle(options, logger));
            builder.Services.AddSingleton(typeof(ICache<>), typeof(Cache<>));

            return builder;
        }

        public static IIdentityServerBuilder AddNCacheCorsPolicyCache<TCorsPolicyService>(
            this IIdentityServerBuilder builder)
            where TCorsPolicyService:ICorsPolicyService
        {
            builder.Services.TryAddTransient(typeof(TCorsPolicyService));
            builder.Services.AddTransient<ICorsPolicyService,
                NCacheCachingCorsPolicyService<TCorsPolicyService>>();
            return builder;
        }

        public static IIdentityServerBuilder 
            AddNCacheProfileServiceCache<TProfileService>(
                this IIdentityServerBuilder builder, 
                Action<ProfileServiceCachingOptions<TProfileService>> 
                                                        optionsBuilder = null)
        where TProfileService : class, IProfileService
        {
            var options = new ProfileServiceCachingOptions<TProfileService>();
            optionsBuilder?.Invoke(options);
            builder.Services.AddSingleton(options);
            builder.Services.TryAddTransient(typeof(TProfileService));
            builder.Services.AddTransient<
                                    IProfileService, 
                                    CachingProfileService<TProfileService>>();
            return builder;
        }

        public static IIdentityServerBuilder 
            AddNCachePersistedGrantStoreCache<TPersistedGrantStore>(
                this IIdentityServerBuilder builder,
                Action<PersistedStoreCachingOptions> 
                        persistedGrantStoreCacheOptionsAction = null)
        where TPersistedGrantStore:IPersistedGrantStore
        {
            var options = new PersistedStoreCachingOptions();
            persistedGrantStoreCacheOptionsAction?.Invoke(options);

            var logger = builder.Services.BuildServiceProvider()
                .GetRequiredService<ILogger<CachedPersistedStoreCacheHandle>>();

            builder.Services.AddSingleton<CachedPersistedStoreCacheHandle>(
                new CachedPersistedStoreCacheHandle(options, logger));
            builder.Services.TryAddTransient(typeof(TPersistedGrantStore));
            builder.Services.AddTransient<
                                        IPersistedGrantStore,
                                        CachingPersistedGrantStore<
                                                    TPersistedGrantStore>>();

            return builder;
        }


        private static IServiceCollection AddConfigurationStoreRepository(
            this IServiceCollection services,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            var storeOptions = new ConfigurationStoreOptions();
            storeOptionsAction?.Invoke(storeOptions);
            services.AddSingleton(storeOptions);
            services.AddSingleton<ConfigurationStoreCacheHandle>(
                new ConfigurationStoreCacheHandle(storeOptions));
            services.AddSingleton<
                IConfigurationStoreRepository<Client>,
                ConfigurationStoreRepository<Client>>();
            services.AddSingleton<
                IConfigurationStoreRepository<ApiResource>,
                ConfigurationStoreRepository<ApiResource>>();
            services.AddSingleton<
                IConfigurationStoreRepository<IdentityResource>, 
                ConfigurationStoreRepository<IdentityResource>>();

            return services;
        }

        private static IServiceCollection AddPersistedStoreRepository(
            this IServiceCollection services,
            Action<PersistedStoreOptions> storeOptionsAction = null)
        {
            var storeOptions = new PersistedStoreOptions();
            storeOptionsAction?.Invoke(storeOptions);
            services.AddSingleton(storeOptions);
            services.AddSingleton<PersistedGrantStoreCacheHandle>(
                new PersistedGrantStoreCacheHandle(storeOptions));
            services.AddSingleton<IPersistedGrantStoreRepository, 
                                        PersistedGrantStoreRepository>();

            return services;
        }

        private static IServiceCollection AddDeviceStoreRepository(
            this IServiceCollection services,
            Action<DeviceStoreOptions> storeOptionsAction = null)
        {
            var storeOptions = new DeviceStoreOptions();
            storeOptionsAction?.Invoke(storeOptions);
            services.AddSingleton(storeOptions);
            services.AddSingleton<DeviceStoreCacheHandle>(
                new DeviceStoreCacheHandle(storeOptions));
            services.AddSingleton<IDeviceStoreRepository,
                                        DeviceCodeStoreRepository>();

            return services;
        }
    }
}
