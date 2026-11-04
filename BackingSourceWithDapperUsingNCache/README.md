# BACKING SOURCE USING [DAPPER](https://github.com/StackExchange/Dapper)

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Projects](#projects)
* [Configure the Sample](#configure-the-sample)
* [Register the Backing Source Providers](#register-the-backing-source-providers)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

- This sample shows how NCache keeps cached items synchronized with the database. NCache loads expired items from the data source with its [*ReadThru*](https://www.alachisoft.com/resources/docs/ncache/prog-guide/read-through-caching.html) feature and updates items with its [*WriteThru*](https://www.alachisoft.com/resources/docs/ncache/prog-guide/write-through-caching.html) feature, without the client having to do either.
	
	The backing source providers use the *Dapper* micro-ORM, to show that you can build NCache backing-source providers with 3rd-party libraries just as you would build a client application.

	*Dapper* is already a very fast micro-ORM. NCache can boost this performance further in the following ways:

	- 	NCache is a distributed cache that can be *scaled out* to absorb the requests that would otherwise reach the database. As the request load increases, you can add more servers to the cache to handle it, without having to *scale up* your database.

	- 	Because it is distributed, NCache also improves performance when multiple instances of an application run behind a load balancer, e.g. server farms or containerized environments, since all instances share the contents of the cache. 
	
		When one client instance queries for results for the first time, subsequent requests from the other instances get the results directly from the cache.

	- 	A common concern with caching is stale data in the cache. The NCache backing source providers, together with database dependency features, keep the cache in sync with the database through NCache's resync feature. You can see this by running the sample and doing the following:

		1. Fetch results from the database and cache them in NCache.
		1. Run *UPDATE* and *DELETE* commands on the database, from another application or directly in *SQL Server Management Studio*.
		1. Fetch the same results again and confirm that the data in the cache matches the database.  

		All the operations that resync the cache with the database run on the server side, without any client involvement.

> **Note:** In NCache SDK 5.3.800, `SqlCacheDependency` and `CacheItem.Dependency` are marked obsolete because the feature is being retired. The sample still builds and runs, but database dependencies will not be available in future versions.

### Prerequisites

- NCache Enterprise 5.3 (tested with `Alachisoft.NCache.SDK` 5.3.800)
- .NET 6.0 SDK and Runtime (for the console application)
- .NET Framework 4.7.2 Developer Pack (for the backing source providers)
- SQL Server (Express or higher) with a database for the sample
- Visual Studio 2022 (or the `dotnet` CLI)

### Projects

| Project | Target | Description |
|---|---|---|
| `ConsoleUI` | `net6.0` | Menu-driven console application that reads and writes customers through NCache (or directly through Dapper) |
| `DapperBackingSource` | `net472` | ReadThru and WriteThru providers that run inside the NCache server |
| `Models` | `netstandard2.0` | Shared `Customer` model |

### Configure the Sample

All settings are in [appsettings.json](./ConsoleUI/appsettings.json). The server and port of the cache go in the `ncache` section, which NCache reads itself, so no `client.ncconf` is needed.

```json
{
  "CacheName": "dappercache",
  "noOfInstances": 50,
  "UseCache": true,

  "ConnectionStrings": {
    "sql": "Server={server}\\{instance};Database={database};User Id={user};Password={password};TrustServerCertificate=True;"
  },

  "ncache": {
    "dappercache": {
      "server": "{cache server IP}",
      "port": 9800
    }
  }
}
```

- *noOfInstances*: the number of rows added to the test table.
- *CacheName*: the name of your cache.
- *ncache*: one entry per cache, keyed by the cache name, with the IP address (`server`) and client port (`port`, default `9800`) of an NCache server hosting the cache.
- *UseCache*: set to *true* to see the NCache backing source in action, or *false* to read directly from the database through *Dapper*. Comparing the two shows the read performance gained with NCache.
- *sql*: the connection string of the SQL Server database. Keep `TrustServerCertificate=True` if SQL Server uses a self-signed certificate (the default for SQL Express); otherwise the login fails with *"The certificate chain was issued by an authority that is not trusted"*.

If *CacheName* is missing, the application stops with a clear error message.

### Register the Backing Source Providers

The cache must have the following providers registered and deployed before you run the sample:
- Read Thru: [ReadThruProvider](./DapperBackingSource/ReadThruProvider.cs)
- Write Thru: [WriteThruProvider](./DapperBackingSource/WriteThruProvider.cs)

First build the solution. The provider files are in `DapperBackingSource\bin\Debug\net472\`. Deploy only these three files; the NCache assemblies are already on the server:
- `Alachisoft.NCache.Samples.Dapper.BackingSources.dll`
- `Alachisoft.NCache.Samples.Dapper.Models.dll`
- `Dapper.dll`

Both providers read a parameter named **`connectionString`**. Set it to the same connection string as in *App.config*.

#### Using NCache Management Center

1. Create a clustered cache with the name given in *App.config* (`cacheId`), and keep it **stopped**.
1. Open the cache's **Backing Source** settings.
1. Enable **Read Through** and add a provider:
	- Provider name: e.g. `DapperReadThruProvider`
	- Library: `Alachisoft.NCache.Samples.Dapper.BackingSources.dll`
	- Class: `Alachisoft.NCache.Samples.Dapper.BackingSources.ReadThruProvider`
	- Parameter: `connectionString` = your connection string
	- Mark it as the default provider.
1. Enable **Write Through** and add a provider the same way, with class `Alachisoft.NCache.Samples.Dapper.BackingSources.WriteThruProvider` (e.g. `DapperWriteThruProvider`).
1. Deploy the three files listed above, save the changes and start the cache.

#### Using NCache PowerShell

```powershell
Import-Module 'C:\Program Files\NCache\bin\tools\ncacheps\ncacheps.psd1'

$bin  = '<solution path>\DapperBackingSource\bin\Debug\net472'
$deps = '<folder containing Alachisoft.NCache.Samples.Dapper.Models.dll and Dapper.dll>'
$cs   = 'Server={server}\{instance};Database={database};User Id={user};Password={password};TrustServerCertificate=True;'

Add-BackingSource -CacheName dappercache -Server <cache server IP> -ReadThru -DefaultProvider `
    -ProviderName DapperReadThruProvider `
    -AssemblyPath "$bin\Alachisoft.NCache.Samples.Dapper.BackingSources.dll" `
    -Class 'Alachisoft.NCache.Samples.Dapper.BackingSources.ReadThruProvider' `
    -Parameters "connectionString=$cs" -DependentAssemblyPath $deps

Add-BackingSource -CacheName dappercache -Server <cache server IP> -WriteThru -DefaultProvider `
    -ProviderName DapperWriteThruProvider `
    -AssemblyPath "$bin\Alachisoft.NCache.Samples.Dapper.BackingSources.dll" `
    -Class 'Alachisoft.NCache.Samples.Dapper.BackingSources.WriteThruProvider' `
    -Parameters "connectionString=$cs" -DependentAssemblyPath $deps

Start-Cache -Name dappercache -Server <cache server IP>
```

Pass the parameter as `connectionString=<value>`, **without** quotes. Quotes are stored as part of the parameter name, and the provider then fails to find its connection string. To change a provider later, stop the cache, run `Remove-BackingSource`, add the provider again and start the cache.

### Build and Run the Sample
    
- Run the *ConsoleUI* application. On startup it:
	- **drops and re-creates** the *dbo.Customers* table in the database given by the *sql* connection string, so do not point it at a database whose *Customers* table you need;
	- enables SQL Server Service Broker on that database, which [SQL dependency](https://www.alachisoft.com/resources/docs/ncache/prog-guide/sql-dependency.html) requires;
	- seeds the table with the number of rows set by *noOfInstances*.
- Pick any item from the menu, then update or delete rows in the database from outside the application to see that the cached items stay up to date. New rows inserted into the database do not invalidate cached data through database dependencies, so expirations are still needed. When an item expires, NCache's resync re-loads fresh data, including any new rows in the result set.
- The time in ms taken by each read operation is printed to the console, so you can compare direct database access with the NCache backing source providers.
- When you are done, select option *7* to exit. If you restart the application, the table is re-created and seeded with the same initial data.
- If something fails, the full exception, including inner exceptions, is printed to the console.

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
