# Sync NCache with PostgreSQL through Notifications
Using NotifyExtensibleDependency and Read-Through

### Table of contents

* [Introduction](#introduction)
* [Postgresql Asynchronous Notifications Support For NCache Synchronisation](#postgresql-asynchronous-notifications-support-for-ncache-synchronisation)
* [Sample Application Structure](#sample-application-structure)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This project highlights the NCache **NotifyExtensibleDependency**, **ReadThruProvider**, **Resync** feature and how it can be used to synchronize NCache with a PostgreSQL database. [**NotifyExtensibleDependency**](https://www.alachisoft.com/resources/docs/ncache/prog-guide/notification-extensible-dependency.html) is a cache synchronization strategy for tackling the stale data problem, to be discussed shortly. Its primary purpose is to give the solution architect the flexibility to integrate a **real-time customized** logic that monitors and processes datastore change notifications **directly** into the cache hosting processes running on the cache servers.

[**ReadThruProvider**](https://www.alachisoft.com/resources/docs/ncache/prog-guide/configure-read-through-provider.html) will use your custom read-through provider to communicate with the back-end data source. Using this you can incorporate your custom logic to load data from the configured data source. In Read-Through Caching, NCache will call your provider to load data from the data source behind the get call, in case of a cache miss.

In this scheme, the developer deploys a provider(NotifiyExtensibleDependencyProvider and ReadThruprovider) containing the datastore state-change monitoring and processing logic behind the depedency along with the lifecycle hooks into the NCache servers. The servers then invoke the dependency-related methods which encapsulate the custom logic and determine whether or not to remove the associated cached item. The main strengths of this feature are as follows:	

- **Stale Data Problem Mitigation**

  Although using NCache with PostgreSQL boosts application performance, there is one issue that needs to be kept in mind. When you start caching with a primary data store such as PostgreSQL, two copies of the same data will be created, one in the primary data store and the other in the cache. Any direct update to the database data could render the cache data stale. With **NotifyExtensibleDependency**, not only are we taking advantage of the increased read performance provided by NCache, but we can also make sure that stale data does not persist in the cache.

  If you have configured the read thru provider you can choose to check the rsync options to true with the NotifyExtensibleDependency so when the object get invalidated or expired NCache automatically loads the data from the database using the configured ReadThruProvider.

- **Native NCache API Support**

  With support built into the NCache core logic, **NotifyExtensibleDependency** extends the power NCache provides when it comes to **high availability**, **reliability** and **scalability** and adds the mechanisms which ensure that cached-data fully agrees with the primary datastore state at all times.

The following are the advantages of NCache together with this feature when used with PostgreSQL:

- **Faster Read Operations**

  Using NCache as your distributed caching solution, application performance is improved since it is an in-memory key-value store which greatly improves data read performance. 
  
- **Improved Scalability**

  Using **NotifyExtensibleDependency**, all the cache synchronization operations are handed over to the clustered cache itself, allowing the clients to focus on the core business logic. 
  
  Not only does this create a clean logical separation of concerns among the NCache client and servers but it also provides improved scalability of the overall system architecture since any increase in notifications load can easily be handled by scaling out the NCache cluster independently of the client-side infrastructure, an important implication for today's cloud-based microservices applications. 

### Postgresql Asynchronous Notifications Support For NCache Synchronisation

The PostgreSQL Notifications system is based on the *LISTEN/NOTIFY* mechanism and is similar to the Pub/Sub Model. 

The client (in our case NCache) registers with a specific channel by calling the [*LISTEN*](https://www.postgresql.org/docs/10/sql-listen.html) command within the [***NotifyExtensibleDependency***](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Runtime.Dependencies.NotifyExtensibleDependency.html) *Initialize* method and then the PostgreSQL connection is put in a wait state by invoking the [***NpgsqlConnection***](https://www.npgsql.org/doc/api/Npgsql.NpgsqlConnection.html) [*Wait*](https://www.npgsql.org/doc/api/Npgsql.NpgsqlConnection.html#Npgsql_NpgsqlConnection_Wait) call.

On the PostgreSQL database side, you need a trigger that invokes a trigger function. By specifying that the trigger should be called AFTER UPDATE and AFTER DELETE, the trigger function sends a JSON payload on a channel. 

Any clients LISTENING on the channel using the previous LISTEN command get the payload and processes it using the information given therein. In our case, we take the payload and if the data matches the information about the cached item the dependency is attached to, we invoke the *DependencyChanged* event handler.

### Sample Application Structure

A sample ***NotifyExtensibleDependency*** implementation given in the sample follows the approach given in the previous section and you can view the source code [here](./src/PostGreSQLNotificationDependency/PostGreSQLDependency.cs).

A sample ***ReadThruProvider*** implementation given in the sample follows the approach given in the previous section and you can view the source code [here](./src/PostGreSQLBackingSourceProvider/PostGreSQLReadThruProvider.cs).

Also given in this project are the scripts for creating the [demo customers table](./Resources/CustomerTable.sql), the [trigger](./Resources/CustomerTableTrigger.sql) to invoke the trigger function for notification and the [trigger function](./Resources/CustomerTriggerFunction.sql) itself. 

Besides these, there are two console applications that reference the [***NotifyExtensibleDependency***](./src/PostGreSQLNotificationDependency/) library (multi-targeted to .NET Framework 4.7.2 and .NET 8). One is [PostGresDependencyNETConsoleUI](./src/PostGresDependencyNETConsoleUI/) which is a .NET Framework 4.7.2 application and the other is [PostGresDependencyNETCOREConsoleUI](./src/PostGresDependencyNETCOREConsoleUI/) which is a .NET 8 application.

These two console applications are identical in their source code and are meant to show that both a .NET client and a .NET Framework client can make use of the same ***NotifyExtensibleDependency*** feature.

Besides these, there are two console applications that use the [***ReadThruProvider***](./src/PostGreSQLBackingSourceProvider/) deployed on the cache. One is [NETConsoleUI](./src/NETConsoleUI/) which is a .NET Framework 4.7.2 application and the other is [NETCOREConsoleUI](./src/NETCOREConsoleUI/) which is a .NET 8 application. These two console applications are identical in their source code and are meant to show that both a .NET client and a .NET Framework client can get the value from datasource automatically and then from cache using ***ReadThruProvider*** feature.

| Project | Target framework | Description |
|---------|------------------|-------------|
| [Entities](./src/Entities/) | .NET Standard 2.0 | `Customer` entity shared by all projects |
| [PostGreSQLNotificationDependency](./src/PostGreSQLNotificationDependency/) | .NET Framework 4.7.2 and .NET 8 (`net472;net8.0`) | ***NotifyExtensibleDependency*** implementation using PostgreSQL LISTEN/NOTIFY |
| [PostGreSQLBackingSourceProvider](./src/PostGreSQLBackingSourceProvider/) | .NET Framework 4.7.2 | ***ReadThruProvider*** implementation, deployed on the cache servers |
| [PostGresDependencyNETConsoleUI](./src/PostGresDependencyNETConsoleUI/) | .NET Framework 4.7.2 | Adds an item with the PostgreSQL dependency |
| [PostGresDependencyNETCOREConsoleUI](./src/PostGresDependencyNETCOREConsoleUI/) | .NET 8 | Adds an item with the PostgreSQL dependency |
| [NETConsoleUI](./src/NETConsoleUI/) | .NET Framework 4.7.2 | Reads an item through the ReadThruProvider |
| [NETCOREConsoleUI](./src/NETCOREConsoleUI/) | .NET 8 | Reads an item through the ReadThruProvider |

All NCache projects use the [Alachisoft.NCache.SDK](https://www.nuget.org/packages/Alachisoft.NCache.SDK/) NuGet package version 5.3.800, and the PostgreSQL projects use [Npgsql](https://www.nuget.org/packages/Npgsql/) 8.0.9.

**NOTE**: NCache 5.3 marks ***NotifyExtensibleDependency*** (along with `ExtensibleDependency`, `CacheDependency` and the `CacheItem.Dependency` property) as obsolete: this feature is being retired and will not be continued in future versions. The sample still works with NCache 5.3, but building it shows obsolete (CS0618) warnings.

### Prerequisites

Before the sample application is executed make sure that:

- NCache Enterprise 5.3 is installed on the cache servers. The latest version can be gotten from [here](https://www.alachisoft.com/download-ncache.html).

  **NOTE**: The rest of this article assumes a Windows cache server running .NET Framework based providers; the server-side assemblies of this sample are built for .NET Framework 4.7.2.

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) and the .NET Framework 4.7.2 Developer Pack (or Visual Studio 2022 with both workloads) are installed on the development machine. The NuGet packages (Alachisoft.NCache.SDK 5.3.800, Npgsql 8.0.9) are restored automatically when the solution is built.
  
- Make sure the relevant ports are open to allow for full communication between NCache and the PostgreSQL Database.
This sample was tested with PostgreSQL version "10.15"

- In your PostgreSQL database, run the SQL ***CREATE*** scripts given in the [Resources](./Resources/) folder to create the *customers* table, the *customers* table trigger that will be invoking the Notifications trigger function, and the trigger function itself.

  **IMPORTANT**:Confirm that the trigger is enabled.

- In both the [*PostGresDependencyNETConsoleUI*](./src/PostGresDependencyNETConsoleUI) and [*PostGresDependencyNETCOREConsoleUI*](./src/PostGresDependencyNETCOREConsoleUI) applications, change the *Program.cs* source code by adding the connection string to the PostgreSQL database that contains the *customers* table and the associate trigger and trigger function as shown below:

  ```csharp

  // Beginning of Program.cs

  // Change the connection string to your PostGreSQL Database
    const string connectionString = "Host={server};Port=5432;Username={user};Password={password};Database={database}";

  // Rest of code
  ```
   
   Furthermore, set the cache name and the IP address and port of one of the servers of your demo cache in each console application. No *client.ncconf* is needed:

   - **.NET 8** apps ([NETCOREConsoleUI](./src/NETCOREConsoleUI/appsettings.json), [PostGresDependencyNETCOREConsoleUI](./src/PostGresDependencyNETCOREConsoleUI/appsettings.json)), in *appsettings.json*:

     ```json
     {
       "CacheName": "democache",
       "ncache": {
         "democache": { "server": "{cache server IP}", "port": 9800 }
       }
     }
     ```

   - **.NET Framework** apps ([NETConsoleUI](./src/NETConsoleUI/App.config), [PostGresDependencyNETConsoleUI](./src/PostGresDependencyNETConsoleUI/App.config)), in *App.config*:

     ```xml
     <configSections>
       <section name="ncache" type="Alachisoft.NCache.Client.NCacheSection, Alachisoft.NCache.Client" />
     </configSections>
     <appSettings>
       <add key="CacheName" value="democache" />
     </appSettings>
     <ncache>
       <caches>
         <add name="democache" server="{cache server IP}" port="9800" />
       </caches>
     </ncache>
     ```

   NCache reads the server and port of the cache from the `ncache` section itself; the applications only read *CacheName*.

   After making the above mentioned changes, build the solution to generate the assemblies.

- Create a cache named *democache* using the [*NCache Web Manager*](https://www.alachisoft.com/resources/docs/ncache/admin-guide/ncache-web-manager.html?tabs=windows%2Cwindows-start-manager) GUI. The steps for doing this are given [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-new-cache-cluster.html?tabs=windows#using-ncache-web-manager).

  **IMPORTANT**: Do not start the cache after creating it as we will be deploying the ***NotificyExtensibleDependency*** implementation dependencies as shown in the next step.

- Once *democache* is created configure the ReadThruProvider using the tutorial mentioned [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/read-through-provider.html). Name the ReadThruProvider with Name "PostGreSqlReadThruProvider", use the class *PostGreSQLBackingSourceProvider.PostGreSQLReadThruProvider* and add **connectionString** as parameter with your PostgreSQL connection string, e.g. "Host={database server};Port=5432;Username={user};Password={password};Database={database}". 

- Once *democache* is created, we need to deploy the assemblies related to PostGreSQLNotificationDependency and PostGreSQLBackingSourceProvider to allow for the PostgreSQL notification logic and ReadThruProvider with PostGreSQL to run on the server side onto *democache*.

  The server-side assemblies are built for .NET Framework 4.7.2. Deploy them from the `bin\Debug\net472` (or `bin\Release\net472`) folder of the [*PostGreSQLBackingSourceProvider*](./src/PostGreSQLBackingSourceProvider/) project; that project copies all of its dependencies to its output folder. Deploy:

  - *PostGreSQLBackingSourceProvider.dll*, *PostGreSQLNotificationDependency.dll* and *Entities.dll*
  - *Npgsql.dll* and its dependencies from the same folder (e.g. *Microsoft.Extensions.Logging.Abstractions.dll*, *Microsoft.Bcl.HashCode.dll*, *System.Threading.Channels.dll*, *System.Text.Json.dll*, *System.Collections.Immutable.dll*, *System.Diagnostics.DiagnosticSource.dll*, *System.Memory.dll*, *System.Runtime.CompilerServices.Unsafe.dll*, *System.Threading.Tasks.Extensions.dll*, *System.Buffers.dll*, *System.Numerics.Vectors.dll*, *System.Text.Encodings.Web.dll*, *Microsoft.Bcl.AsyncInterfaces.dll*)
  - *Newtonsoft.Json.dll* only if your NCache installation does not already provide it

  Do **not** deploy the *Alachisoft.NCache.\** assemblies; they are already part of the NCache installation.

  Once the assemblies are deployed using the steps given [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/deploy-providers.html), start the cache.
		
- On the database side, run an ***INSERT*** script on the *customers* table to insert a row with *customerid* field set to *ALFKI*. We will applying ***UPDATE*** and ***DELETE*** calls on this field to demonstrate real-time cache-invalidation on the NCache side using the ***NotifyExtensibleDependency*** implementation deployed with the assemblies during the previous step.

### Build and Run the Sample
    
- Run the *PostGresDependencyNETConsoleUI* application. This is will insert a dummy cache item in the cache with dependency set on it. This dependency is an instance of the ***NotifyExtensibleDependency*** implementation as shown [here](./src/PostGreSQLNotificationDependency/PostGreSQLDependency.cs).

- In the [*Statistics*](https://www.alachisoft.com/resources/docs/ncache/admin-guide/browse-cache-statistics.html?tabs=windows) page of the NCache Web Manager for *democache*, the *Count* field should now be *1*, signifying that the item was successfully added along with the dependency on the PostgreSQL database.

- On the database side, run either an ***UPDATE*** or a ***DELETE*** command script on the *customers* table for row with *customerid* field set to *ALFKI*. Once the transaction is complete, NCache will receive a notification from PostgreSQL using ***NOTIFY*** command within the trigger function that we created.

  In turn, the cached item will be invalidated and removed from cache, as demonstrated by the *Count* field in the *NCache Web Manager* *Statistics* page for *democache* going back to *0*.

- Repeat the steps for the *PostGresDependencyNETCOREConsoleUI* application to confirm that the same behavior is observed when data is added from a .NET 8 client application.

- To try Read-Through, run the *NETConsoleUI* or *NETCOREConsoleUI* application. It fetches the customer *ALFKI* through the *PostGreSqlReadThruProvider*, which loads it from PostgreSQL with the PostgreSQL dependency and resync enabled.

### Additional Resources

##### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

##### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

### Technical Support

Alachisoft &copy; provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

&copy; 2020 Alachisoft 
