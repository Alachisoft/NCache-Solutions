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
The wrapper connects to the cache server directly (thin client), so no `client.ncconf` is needed. Pass the cache name together with the address of an NCache server hosting the cache:

```csharp
// Cache server IP and client port (default 9800)
ConnectionMultiplexer ncache = ConnectionMultiplexer.Connect("myCache", "{cache server IP}", 9800);

// Or with full NCache connection options
var options = new ConnectionOptions
{
    Servers = new List<ServerInfo> { new ServerInfo("{cache server IP}", 9800) }
};
ConnectionMultiplexer ncache2 = ConnectionMultiplexer.Connect("myCache", options);

var db = ncache.GetDatabase();
```

`ConnectionOptions` and `ServerInfo` are in the `Alachisoft.NCache.Client` namespace. The `Connect(string cacheName, CacheConnectionOptions options)` overload has been replaced by `Connect(string cacheName, ConnectionOptions options)`, and `Connect(string cacheName)` (which relied on `client.ncconf`) has been removed.

## Configure the Sample
The sample reads its settings from [App.config](./NCache.StackExchange.Redis.Sample/App.config):

```xml
<appSettings>
  <add key="cacheName" value="cacheName"/>
  <add key="cacheServer" value="{cache server IP}"/>
  <add key="cachePort" value="9800"/>
</appSettings>
```

- *cacheName*: the name of your cache.
- *cacheServer* / *cachePort*: the IP address and client port (default `9800`) of an NCache server hosting the cache.

If any of these settings is missing, the sample stops with a clear error message.

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