# NCache.StackExchange.Redis (NCache wrapper of StackExchange.Redis)
## Introduction
This solution will allow you to use NCache using the StackExchange.Redis APIs. This is not a redis client rather this is an NCache .Net client with StackExchange.Redis API's. This will help those who want to use NCache with minimal code change if they are already familiar with StackExchange.Redis (.Net client of redis). 
# Technology
NCache.StackExchange.Redis assembly is built with target framework ".NET 6.0" (`net6.0`), because the NCache SDK 5.3.800 does not ship a .NET Standard build.

## Prerequisites
- NCache Enterprise 5.3 (tested with `Alachisoft.NCache.SDK` 5.3.800)
- .NET 6.0 SDK and Runtime
- Visual Studio 2022 (or the `dotnet` CLI)

## No Major Code change
To use this wrapper a sample project "NCache.StackExchange.Redis.Sample" is also provided which will let you play with NCache even more quickly.

* The code which is previously using StackExchange.Redis to interact with redis servers just need to remove the StackExchange.Redis Nuget and Use NCache.StackExchange.Redis.
* Namespace change will be from "StackExchange.Redis" to "NCache.StackExchange.Redis"

## Connecting to the Cache
Pass the cache name. NCache reads the server and port of the cache from the `ncache` section of the application's *appsettings.json* (or the `<ncache>` section of *App.config* / *Web.config* for .NET Framework), so no `client.ncconf` is needed:

```csharp
ConnectionMultiplexer ncache = ConnectionMultiplexer.Connect("myCache");
var db = ncache.GetDatabase();
```

The `Connect(string cacheName, CacheConnectionOptions options)` overload has been replaced by `Connect(string cacheName, ConnectionOptions options)` (`Alachisoft.NCache.Client` namespace) for any additional connection settings.

## Configure the Sample
The sample reads its settings from [appsettings.json](./NCache.StackExchange.Redis.Sample/appsettings.json):

```json
{
  "CacheName": "demoCache",
  "ncache": {
    "demoCache": {
      "server": "{cache server IP}",
      "port": 9800
    }
  }
}
```

- *CacheName*: the name of your cache.
- *ncache*: one entry per cache, keyed by the cache name, with the IP address (`server`) and client port (`port`, default `9800`) of an NCache server hosting the cache.

If *CacheName* is missing, the sample stops with a clear error message.

> **Warning:** the sample calls `Clear()` on the cache at startup, which removes **all** of its data. Do not point it at a cache that holds data you need.


## Additional Resources

#### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

#### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

## Technical Support

Alachisoft [C] provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

## Copyrights

[C] Copyright 2020 Alachisoft