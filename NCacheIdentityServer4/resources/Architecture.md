# NCache IdentityServer4 Storage and Caching Provider Architecture

### Table of contents

* [NCache as In-Memory IdentityServer4 Store](#ncache-as-in-memory-identityserver4-store)
* [Configuration for NCache IdentityServer4 Store](#configuration-for-ncache-identityserver4-store)
* [NCache as IdentityServer4 Cache Implementation](#ncache-as-identityserver4-cache-implementation)
* [Connection Parameters for Accessing NCache Cluster](#connection-parameters-for-accessing-ncache-cluster)
* [Registering NCache in IdentityServer4 Application](#registering-ncache-in-identityserver4-application)


### NCache as In-Memory IdentityServer4 Store

The storage architecture of NCache as an IdentityServer4 store closely follows that of the EntityFramework implementation provided [here](https://github.com/IdentityServer/IdentityServer4/tree/master/src/EntityFramework.Storage/src)
and as such, NCache as a storage mechanism for IdentityServer4 entities can be broken down into the following categories:

- Configuration Store

   NCache can be used a configuration store for storing and accessing IdentityServer4 Clients, API Resources as well as Identity Resources. For this purpose, the following IdentityServer4
   interfaces have been implemented:
   
   - [IClientStore](https://github.com/IdentityServer/IdentityServer4/blob/main/src/Storage/src/Stores/IClientStore.cs)
   
     The implementation of the **IClientStore** interface can be found [here](../src/IdentityServer4.NCache/PersistantStorage/Stores/ClientStore.cs). The constructor takes an instance of 
	 [**IConfigurationStoreRepository**](../src/IdentityServer4.NCache/PersistantStorage/Interfaces/IConfigurationStoreRepository.cs) using dependency injection. This instance is configured for 
	 the actual NCache CRUD operations on the NCache cluster designated as a configuration store and serves the same role as the [**IConfigurationDbContext**](https://github.com/IdentityServer/IdentityServer4/blob/master/src/EntityFramework.Storage/src/Interfaces/IConfigurationDbContext.cs) 
	 entity in the IdentityServer4 EF Core storage framework. The implementation of the **IConfigurationStoreRepository** interface can be viewed [here](../src/IdentityServer4.NCache/PersistantStorage/Repositories/ConfigurationStoreRepository.cs).

   - [IResourceStore](https://github.com/IdentityServer/IdentityServer4/blob/main/src/Storage/src/Stores/IResourceStore.cs)	  
   
     The implementation of the **IResourceStore** interface can be found [here](../src/IdentityServer4.NCache/PersistantStorage/Stores/ResourceStore.cs). As with the 
	 **IClientStore** implementation, the constructor takes an instance of [**IConfigurationStoreRepository**](../src/IdentityServer4.NCache/PersistantStorage/Interfaces/IConfigurationStoreRepository.cs) 
	 using dependency injection.
	  
- Operational Store

	NCache can be used a operational store for storing and accessing IdentityServer4 persisted grants and device codes. For this purpose, the following IdentityServer4
    interfaces have been implemented:
   
   - [IPersistedGrantStore](https://github.com/IdentityServer/IdentityServer4/blob/3b717b15300db9d10d7328a1087a1bf778331010/src/Storage/src/Stores/IPersistedGrantStore.cs)
   
     The implementation of the **IPersistedGrantStore** can be found [here](../src/IdentityServer4.NCache/PersistantStorage/Stores/PersistedGrantStore.cs). The constructor takes an instance of 
	 [**IPersistedGrantStoreRepository**](../src/IdentityServer4.NCache/PersistantStorage/Interfaces/IOperationalStoreRepository.cs) using dependency injection. This instance is configured for 
	 the actual NCache CRUD operations on the NCache cluster designated as a operational store and serves the same role as the [**IPersistedGrantDbContext**](https://github.com/IdentityServer/IdentityServer4/blob/master/src/EntityFramework.Storage/src/Interfaces/IPersistedGrantDbContext.cs) 
	 entity in the IdentityServer4 EF Core storage framework. The implementation of the **IPersistedGrantStoreRepository** interface can be viewed [here](../src/IdentityServer4.NCache/PersistantStorage/Repositories/PersistedGrantStoreRepository.cs).
	  
   - [IDeviceFlowStore](https://github.com/IdentityServer/IdentityServer4/blob/main/src/Storage/src/Stores/IDeviceFlowStore.cs)
   
     The implementation of the **IDeviceFlowStore** interface can be found [here](../src/IdentityServer4.NCache/PersistantStorage/Stores/DeviceFlowStore.cs). As with the 
	 **IPersistedGrantStore** implementation, the constructor takes an instance of [**IDeviceStoreRepository**](../src/IdentityServer4.NCache/PersistantStorage/Interfaces/IOperationalStoreRepository.cs)
	 using dependency injection. The implementation of the **IDeviceStoreRepository** can be found [here](../src/IdentityServer4.NCache/PersistantStorage/Repositories/DeviceCodeStoreRepository.cs).
	 
The idea behind using independent entities for storing and accessing the configuration and operational data pertinent to IdentityServer4 is to give the flexibility of using NCache optionally as either
a configuration store or an operational store and utilizing a different storage provider alongside it. 

Ofcourse, you can use NCache for both configuration and operational entity
storage and use a single NCache cluster or separate NCache clusters for both categories of IdentityServer4 data.

You can find a demonstration of how NCache can be registered as an IdentityServer4  configuration and operational store [here](../src/Sample/IdentityServer/StartupNCache.cs).


### Configuration for NCache IdentityServer4 Store

  - Configuration Store

    For using NCache as a configuration store, you can use the [**ConfigurationStoreOptions**](../src/IdentityServer4.NCache/Options/ConfigurationStoreOptions.cs) class whose inherted properties from [**OptionsBase**](#connection-parameters-for-accessing-ncache-cluster) class determine the connection parameters to the NCache configuration store.
	
	
   
  - Operational Store

    For using NCache as a **IPersistedGrantStore** provider, you can use the [**PersistedStoreOptions**](../src/IdentityServer4.NCache/Options/PersistedStoreOptions.cs) class whose inherted properties from **OptionsBase** class determine the connection parameters to the NCache persisted grant store.

    For using NCache as a **IDeviceFlowStore** provider, you can use the [**DeviceStoreOptions**](../src/IdentityServer4.NCache/Options/DeviceStoreOptions.cs) class whose inherted properties from **OptionsBase** class determine the connection parameters to the NCache persisted grant store. 
	
An example of registering NCache as an IdentityServer4 Configuration and Operational Store can be found [here](../src/Sample/IdentityServer/StartupNCache.cs).


### NCache as IdentityServer4 Cache Implementation

- ICache\<T> 

  Besides using NCache as an IdentityServer4 configuration and/or operational store as mentioned [before](#ncache-as-in-memory-identityserver4-store), NCache can also be used in the implementation of [**ICache\<T>**](https://github.com/IdentityServer/IdentityServer4/blob/master/src/IdentityServer4/src/Services/ICache.cs) for caching purposes as explained in the IdentityServer4 [docs](http://docs.identityserver.io/en/3.1.0/topics/startup.html#caching). The implementation of the **ICache\<T>**
interface can be found [here](../src/IdentityServer4.NCache/Caching/Cache.cs). 

  The parameters to connect to the NCache cluster used as an **ICache\<T>** implementation is encapsulated within the [**ICacheOptions**](../src/IdentityServer4.NCache/Options/ICacheOptions.cs) class. Besides the properties inherited from the [**OptionsBase**](#connection-parameters-for-accessing-ncache-cluster) class, the **ICacheOptions** class has the following configurable properties:

  - Expiration (Optional)

    This is an instance of the **CacheExpirationType** enum that has the following two possible values: *Absolute* and *Sliding*. As such, we can configuration the **ICache\<T>** implementation to cache the values with absolute or sliding expirations. **The default value is CacheExpirationType.Absolute**.

  - Circuit-Breaker Related Properties

    For resilience, the **ICache\<T>** implementation has incorporated the [circuit breaker](https://docs.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker) pattern so that in case there are any network-related issues preventing the use of NCache as a caching layer, the application can still proceed by accessing the data from the underlying database. 

    For that purpose, we have the following two configurable properties to determine the circuit breaker logic:

    - ExceptionsAllowedBeforeBreaking (Optional)

      This determines how many times an exception can be thrown when perform the underlying NCache operations before the circuit breaker goes into the *Open* state. The circuit breaker specifically measures how many times we get an NCache exception regarding inability to find an accessible NCache cluster server. **The dafault value is 1**.

      In case the circuit is open, the default fallback value is returned that indicated broken circuit and the logic can then determine to resort to getting the data from the underlying datastore without the application crashing.

    - DurationOfBreakInSeconds (Optional)

      This determines the amount of time, **in seconds**, that the circuit breaker will stay in the *Open* state before transitioning to the *Half-Open* state. **The default value is 30**.



- Caching IPersistedGrantStore Data

  You can also use the CachePersistedGrantStore found [here](../src/IdentityServer4.NCache/Caching/PersistedGrantStore/CachingPersistedGrantStore.cs) to cache operational store data as well. The parameters to connect to the NCache cluster used for caching the **IPersistedGrantStore** data is contained within the [**PersistedStoreCachingOptions**](../src/IdentityServer4.NCache/Options/PersistedStoreCachingOptions.cs) class. 
  
  Besides the properties inherited from **ICacheOptions** class, it has the following properties determining the cached persisted grant entity expiration behavior:

  - UsePersistedGrantExpiration (Optional)

    Setting this property to *true* binds the lifetime of the cached persisted grant entity to the *Expiration* (if given) property of the entity, effectively persisting the entity in the cache for as long as it is valid (not expired). **The default value is false**.

  - TimeSpanInSeconds (Optional)

    In case the *UsePersistedGrantExpiration* is set to *false*, this value determines the duration, **in seconds**, that the cached entity will persist in the cache, either in *Sliding* or *Absolute* mode, as determined by the *Expiration* inherited property. **The default value is 600 (10 min)**.

- Caching IProfileService Data

  You can use NCache as a caching mechanism for the **IProfileService** as well. The parameters to connect to the NCache cluster responsible for caching **IProfileService** data can be found in the [**ProfileServiceCachingOptions**](../src/IdentityServer4.NCache/Options/ProfileServiceCachingOptions.cs) class. 

  You can use the parameters to select which users to cache based on the **ShouldCache** property. The **Expiration** property determines the duration the data should remain in the cache.

- Caching ICorsPolicyService Data

  You can use NCache to cache **ICorsPolicyService** data by using the [*AddNCacheCorsPolicyCache*](#registering-ncache-in-identityserver4-application) extension method on the **IIdentityServerBuilder*. 

You can find a demonstration of how NCache can be registered as an IdentityServer4 caching mechanism [here](../src/Sample/IdentityServer/StartupEFCore.cs).




### Connection Parameters for Accessing NCache Cluster

The parameters needed to connect to an NCache cluster, whether as a configuration and/or operational store or whether NCache as a caching layer on top of existing IdentityServer4 storage media, is encapsulated in the [**OptionsBase**](../src/IdentityServer4.NCache/Options/OptionsBase.cs) class. The two properties of the class are as follows:

- CacheId (**Required**)

  This is the cache id of the cache cluster to be used as a store or cache. This value is required and defaults to *default*. This must be changed by the user according to the name of the cache cluster used.

- ConnectionOptions (**Optional**)

  The [**NCacheConnectionOptions**](../src/IdentityServer4.NCache/Options/NCacheConnectionOptions.cs) class is a thin wrapper around the [**CacheConnectionOptions**](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Client.CacheConnectionOptions.html) whose properties are mapped directly to the inner **CacheConnectionOptions** instance. 
  
  This is an optional parameter that is used to override some or all the parameters in the [**client.ncconf**](https://www.alachisoft.com/resources/docs/ncache-pro/admin-guide/client-config.html) file. If the information regarding connection to the NCache cluster is available in **client.ncconf** file, whether it is in the project folder or whether it is the NCache installation directory in the **Config** subfolder, then this property need not be set. 

  **Warning**: If the **client.ncconf** files in the project folder or the installation directory do not contain the information regarding the cache cluster in question, then the **ConnectionOptions** property **must** be **atleast** set with the information of **atleast** one of the servers making up the NCache cluster. That information is provided within the **ServerList** property of the **NCacheConnectionOptions** class. 




### Registering NCache in IdentityServer4 Application

IdentityServer4 infrastructure is built up using extension methods on  the [**IIdentityServerBuilder**](https://github.com/IdentityServer/IdentityServer4/blob/master/src/IdentityServer4/src/Configuration/DependencyInjection/IIdentityServerBuilder.cs) interface. 

The extension methods used to register NCache as a storage and cache medium can be found [here](../src/IdentityServer4.NCache/IdentityServerNCacheBuilderExtensions.cs). Here is a summary of the extension methods:

- NCache as an IdentityServer4 Configuration and Operational Store

  - *AddNCacheConfigurationStore*

    This **IIdentityServerBuilder** extension method is used to register NCache as an IdentityServer4 configuration store that contains information about IdentityServer4 clients and resources (api and identity). This registers the NCache **IClientStore**, **IResourceStore** and **ICorsPolicyService** implementations. This takes as an optional parameter of *Action\<[ConfigurationStoreOptions](../src/IdentityServer4.NCache/Options/ConfigurationStoreOptions.cs)>*.

  - *AddNCachePersistedGrantStore*

    This **IIdentityServerBuilder** extension method is used to register NCache as an IdentityServer4 persisted grant store that contains information about IdentityServer4 persisted grants and as such registers the NCache **IPersistedGrantStore** implementation. This takes as an optional parameter of *Action\<[PersistedStoreOptions](../src/IdentityServer4.NCache/Options/PersistedStoreOptions.cs)>*.

  - *AddNCacheDeviceCodeStore*

    This **IIdentityServerBuilder** extension method is used to register NCache as an IdentityServer4 device flow store that contains information about IdentityServer4 device flow codes and as such registers the NCache **IDeviceFlowStore** implementation. This takes as an optional parameter of *Action\<[DeviceStoreOptions](../src/IdentityServer4.NCache/Options/DeviceStoreOptions.cs)>*.

- NCache as a Caching Layer for Underlying IdentityServer4 Store Media

  - *AddNCacheCaching*

    This **IIdentityServerBuilder** extension method is used to register the NCache **ICache\<T>** implementation. This takes an optional parameter of *Action\<[ICacheOptions](../src/IdentityServer4.NCache/Options/ICacheOptions.cs)>*.

  - *AddNCachePersistedGrantStoreCache\<TPersistedGrantStore>*

    This **IIdentityServerBuilder** extension method is used to register the NCache caching layer on top of the *TPeristedGrantStore* implementation of **IPersistedGrantStore**. This takes as an optional parameter of *Action\<[PersistedStoreCachingOptions](../src/IdentityServer4.NCache/Options/PersistedStoreCachingOptions.cs)>*.

  - *AddNCacheProfileServiceCache\<TProfileService>*

    This **IIdentityServerBuilder** extension method is used to register the **IProfileService** cache using **ICache\<T>** dependency  where *TProfileService* is the underlying implementation of **IProfileService**. It takes an optional parameter of *Action\<[ProfileServiceCachingOptions<TProfileService>](../src/IdentityServer4.NCache/Options/ProfileServiceCachingOptions.cs)>*.

  - *AddNCacheCorsPolicyCache\<TCorsPolicyService>*

    This IIdentityServerBuilder extension is used to register the **ICorsPolicyService** cache using **ICache\<T>** dependency where *TCorsPolicyService* is the underlying implementation of **ICorsPolicyService**. **Use this method if registering NCache as an **ICache\<T>** implementation**. Due to behavior similar to [bug](https://github.com/IdentityServer/IdentityServer4/issues/3780), using the IdentityServer4 built-in *AdCorsPolicyCache* doesn't work.

    

  