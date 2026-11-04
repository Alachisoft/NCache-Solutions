# NCache IdentityServer4 Storage and Caching Provider

### Table of contents

* [Introduction](#introduction)
* [Steps Before Starting Application](#steps-before-starting-application)
* [NCache as In-Memory IdentityServer4 Store](#ncache-as-in-memory-identityserver4-store)
* [NCache as IdentityServer4 Cache Implementation](#ncache-as-identityserver4-cache-implementation)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This sample is taken from the IdentityServer4 GitHub [Samples illustrating the use of EntityFramework Core with IdentityServer4](https://identityserver4.readthedocs.io/en/latest/quickstarts/5_entityframework.html) 
which can be found [here](./src/Sample) and it is made up of 4 applications to showcase typical IdentityServer4 behavior. 
The basic working of the EntityFramework Core sample is explained in the IdentityServer4 docs page given [here](https://identityserver4.readthedocs.io/en/latest/quickstarts/5_entityframework.html).

All projects target **.NET 6** and use the **NCache SDK 5.3.800** (`Alachisoft.NCache.SDK`): the cache name and the cache server 
are given in *appsettings.json* and no *client.ncconf* file is needed.

> **Note on IdentityServer4 version**: The sample uses **IdentityServer4 3.1.3** (with IdentityServer4.EntityFramework 3.1.3). IdentityServer4 
> has reached **end-of-life** and no longer receives updates or security fixes. Version 3.1.3 targets .NET Core 3.1 and runs on .NET 8. 
> Because IdentityServer4.EntityFramework 3.1.3 is built against EF Core 3.1, the sample uses **EF Core 3.1.x** (not EF Core 8). 
> For production use, consider moving to a supported product such as [Duende IdentityServer](https://duendesoftware.com/products/identityserver).

### Steps Before Starting Application

As mentioned in the [introduction](#introduction), the sample has been taken from the IdentityServer4 Quickstarts section and as such, the applications will run as expected from a single machine. 

**However**, in case you would like to spread out the applications on different machines, then the following steps must be taken:

- In the [*Config.cs*](./src/Sample/IdentityServer/Config.cs) file of the [*IdentityServer*](./src/Sample/IdentityServer) project, update the *RedirectUris*, *PostLogoutRedirectUris* and *AllowedCorsOrigins* properties of the MVC and Javascript clients by replacing the URLs on which the client applications will be launched as shown below:

  ```csharp
  public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,
                
                    // where to redirect to after login
                    RedirectUris = 
                    { "http://<MVC URL>/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = 
                    { "http://<MVC URL>/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },

                    AllowOfflineAccess = true
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           
                    { "http://<JS Client URL>/callback.html" },
                    PostLogoutRedirectUris = 
                    { "http://<JS Client URL>/index.html" },
                    AllowedCorsOrigins =     
                    { "http://<JS Client URL>" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                }
            };
    }

  ```

- In the [*Startup.cs*](./src/Sample/Api/Startup.cs) file of the [*Api*](./src/Sample/Api) project, update the code as shown below:

  ```csharp
  public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = 
                        "http://<IdentityServer URL>";
                    options.RequireHttpsMetadata = false;

                    options.Audience = "api1";
                });

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(
                        "http://<JS Client URL>")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }

  ```
- In the [Startup.cs](./src/Sample/MvcClient/Startup.cs) file of the [*MvcClient*](./src/Sample/MvcClient) project, update the URL of the IdentityServer as shown below:

  ```csharp
   services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = 
                    "http://<IdentityServer URL>";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";

                    options.SaveTokens = true;

                    options.Scope.Add("api1");
                    options.Scope.Add("offline_access");
                });
  ```
- In the [*app.js*](./src/Sample/JavaScriptClient/wwwroot/app.js) file of the [JavaScriptClient](./src/Sample/JavaScriptClient) project, change the code as shown below:

  ```javascript
  var config = {
    authority: "http://<IdentityServer URL>",
    client_id: "js",
    redirect_uri: "http://<JS Client URL>/callback.html",
    response_type: "code",
    scope:"openid profile api1",
    post_logout_redirect_uri : "http://<JS Client URL>/index.html",
  };
  ```
With these changes in place, you can now deploy your applications on the different machines for testing NCache as both a [configuration and operational store](#ncache-as-in-memory-identityserver4-store) as well as a [distributed caching mechanism on top of existing storage media](#ncache-as-identityserver4-cache-implementation).


### NCache as In-Memory IdentityServer4 Store

We have modified the sample to demonstrate how NCache can be used as a configuration store to contain data regarding **clients**, 
**api resources**, **identity resources** and/or an operational store to house **persisted grants** and **device flow codes**. 

To demonstrate NCache as an IdentityServer4 store for configuration and operational data, make the following changes in the code:

- In [**Program.cs**](./src/Sample/IdentityServer/Program.cs), modify the code shown below by uncommenting *webBuilder.UseStartup\<StartupNCache>()*
  and commenting out *webBuilder.UseStartup\<StartupEFCore>()* as shown below.


  ```csharp
  // beginning of the code
  .ConfigureWebHostDefaults(webBuilder =>
  {
      webBuilder.UseStartup<StartupNCache>();
      //webBuilder.UseStartup<StartupEFCore>();
  });
  // rest of the code
  ```
  
- In the [**appsettings.json**](./src/Sample/IdentityServer/appsettings.json) file, set the NCache connection information as shown below. 
  *CacheName* is the name of the cache you will be using (in the sample, *democache*). The `ncache` section, which NCache reads itself, 
  has one entry per cache with the IP address of an NCache server hosting that cache (`server`) and the NCache client port (`port`, default *9800*).

  ```json
  "CacheName": "democache",
  "ncache": {
    "democache": {
      "server": "{cache server IP}",
      "port": 9800
    }
  },
  ```
  
- Run applications [**IdentityServer**](./src/Sample/IdentityServer), [**MvcClient**](./src/Sample/MvcClient), [**Api**](./src/Sample/Api), [**JavaScriptClient**](./src/Sample/JavaScriptClient) to see how NCache operates as an IdentityServer4 configuration and operational store after making sure that the demonstration cache used as both a configuration and operational store is running and can be connected to from **IdentityServer** sample
  application.
  
  
### NCache as IdentityServer4 Cache Implementation

Besides NCache available for use as a configuration and operation store, it can also be used to **cache** Client and Resource configuration data by using the 
ICache<T> implementation given [here](./src/IdentityServer4.NCache/Caching/Cache.cs). 
It can also be used to cache operational data using [CachingPersistedGrantStore](./src/IdentityServer4.NCache/Caching/PersistedGrantStore/CachingPersistedGrantStore.cs)
as well as a caching mechanism for the [IProfileService](http://docs.identityserver.io/en/latest/reference/profileservice.html) whose implementation is given
[here](./src/IdentityServer4.NCache/Caching/ProfileService/CachingProfileService.cs).

To demonstrate NCache as a configuration store cache while persisting configuration and operational data in SQL Server, make the following changes in the code:

- In [**Program.cs**](./src/Sample/IdentityServer/Program.cs), modify the code shown below by commenting out *webBuilder.UseStartup\<StartupNCache>()*
  and uncommenting *webBuilder.UseStartup\<StartupEFCore>()* as shown below (this is the default).


  ```csharp
  // beginning of the code
  .ConfigureWebHostDefaults(webBuilder =>
  {
      //webBuilder.UseStartup<StartupNCache>();
      webBuilder.UseStartup<StartupEFCore>();
  });
  // rest of the code
  ```
 
- In the [**appsettings.json**](./src/Sample/IdentityServer/appsettings.json) file, set the NCache connection information (*CacheName* 
  and the `ncache` section) as shown below, and give the connection string of the SQL Server in *ConnectionStrings:db*.

  ```json
  "CacheName": "democache",
  "ncache": {
    "democache": {
      "server": "{cache server IP}",
      "port": 9800
    }
  },

  "ConnectionStrings": {
    "db": "server=tcp:{SQL Server IP},1433;database=IdentityServer4.EntityFramework;User Id={user};Password={password};"
  }
  ```
  
  **IMPORTANT**: The application will generate the database and all relevant tables using EF Core migrations using that connection string. Don't create
  the database beforehand.
  
- Run applications [**IdentityServer**](./src/Sample/IdentityServer), [**MvcClient**](./src/Sample/MvcClient), [**Api**](./src/Sample/Api), [**JavaScriptClient**](./src/Sample/JavaScriptClient) to see how NCache operates as a caching mechanism for the configuration store, the persisted grant store as well as the IProfileService default implementation after making sure that the demonstration cache used as a configuration store and persisted grant store cache is running and can be connected to from **IdentityServer** sample application.
   

### Prerequisites

Requirements:

- .NET 6 SDK and runtime for building and running the applications comprising the sample.
- NCache 5.3 running on server(s). The sample uses the [Alachisoft.NCache.SDK](https://www.nuget.org/packages/Alachisoft.NCache.SDK/) NuGet package, version 5.3.800.
- SQL Server (required when using *StartupEFCore*, the default), reachable from the **IdentityServer** sample application.

Before the sample application is executed make sure that:
- appsettings.json have been changed according to the configurations. 
	- Change the cache name (*CacheName*) and the cache server IP address and port in the `ncache` section.
	- Change the SQL Server connection string (*ConnectionStrings:db*).
- Make sure that cache is running. 
- The cache server must be reachable from the client machine on the NCache client port (default 9800).


### Build and Run the Sample
    
- Run the sample applications.

### Additional Resources

##### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

Documentation regarding the architecture of the NCache IdentityServer4 provider is available [here](./resources/Architecture.md)

##### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

### Technical Support

Alachisoft &copy; provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

&copy; 2020 Alachisoft 
