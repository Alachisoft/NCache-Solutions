# NCache.StackExchange.Redis (NCache wrapper for StackExchange.Redis)

## Table of Contents
* [Introduction](#introduction)
* [Technology](#technology)
* [How to Use NCache StackExchange.Redis Wrapper](#how-to-use-ncache-stackexchange.redis-wrapper)
* [How to Run the Sample Application](#how-to-run-the-sample-application)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

## Introduction
This solution will allow you to use NCache using the StackExchange.Redis APIs. This is not a redis client rather this is an NCache .NET client with StackExchange.Redis APIs. This will help those who want to use NCache with minimal code change if they are already familiar with StackExchange.Redis (.NET client for Redis). 

## Technology
NCache.StackExchange.Redis assembly is built with targeted framework ".NETStandard 2.0".

## How to Use NCache StackExchange.Redis Wrapper
Follow the minimalistic instructions given below to use NCache as a caching solution for your existing StackExchange.Redis application:

- **STEP 1:**

  Install [StackExchange.Redis.Wrapper.NCache](https://www.nuget.org/packages/StackExchange.Redis.Wrapper.NCache/) NuGet package once you have removed StackExchange.Redis NuGet package from your existing application. After installing the NuGet package in your StackExchange.Redis application, [**client.ncconf**](https://www.alachisoft.com/resources/docs/ncache/admin-guide/client-config.html) and [**config.ncconf**](https://www.alachisoft.com/resources/docs/ncache/admin-guide/config-ncconf.html) files will be included in your project. 

  The project **client.ncconf** file is used to configure out-of-process local caches and clustered caches, the **config.ncconf** files used in the cache servers of your cache cluster will reside in the **%NCHOME%Config** folder of each of the servers. Here **%NCHOME%** refers to the NCache installation directory e.g. **C:\Program Files\NCache**.

-  **STEP 2:**

   Replace the ```StackExchange.Redis``` namespace with ```NCache.StackExchange.Redis``` in the ***using*** statements of your source files.

- **STEP 3:**

  Replace your ***Server IP*** with the name of cache to be used or add the following key and value to the ***appSettings*** section of your *app.config* or *web.config* file and replace the value *"democache"* with the name of your cache:

  ```xml
   <add key="CacheId" value="democache"/>
  ```
    This is the Id of the cache that will be used. Make sure that the **client.ncconf** file has an entry with this Id so as to be able to configure a cache client to access the cache. This is required if your application is explicitly calling a named cache. 

    In your existing application, using the [**ConnectionMultiplexer**](./NCache.StackExchange.Redis/ConnectionMultiplexer.cs) 
  ***Connect(string configuration)*** method, use ***CacheId*** string value as the argument instead of *Server IP*.

- **STEP 4:**

   Make sure that the cache with the Id given in the **client.ncconf** file with the given cache server configurations (IP addresses and ports) is created and running.

Following these steps, you should now be able to run your StackExchange.Redis application using NCache as the distributed caching solution. To use NCache StackExchange.Redis Wrapper, a sample project "[NCache.StackExchange.Redis.Sample](./NCache.StackExchange.Redis.Sample)" is also provided which will let you play with NCache even more quickly.

## How to Run the Sample Application

In order to demonstrate the use of the NCache StackExchange.Redis Wrapper library, you can use the accompanying [sample](./NCache.StackExchange.Redis.Sample).
The sample uses a local cache configured using the project [**client.ncconf**](./NCache.StackExchange.Redis.Sample/client.ncconf) file.

Before running the application, modify the project [**client.ncconf**](./NCache.StackExchange.Redis.Sample/client.ncconf) file by replacing *testcache* value in the cache id attribute with the name of the clustered cache and replace the server name attribute with the IP address of one of the cache servers.

```xml
<?xml version="1.0" encoding="UTF-8"?>
  <configuration>
    <ncache-server 
      connection-retries="5" 
      retry-connection-delay="0" 
      retry-interval="1" 
      command-retries="3" 
      command-retry-interval="0.1" 
      client-request-timeout="90" 
      connection-timeout="5" 
      port="9800"/>
    <cache 
      id="testcache" 
      client-cache-id="" 
      client-cache-syncmode="optimistic" 
      default-readthru-provider="" 
      default-writethru-provider="" 
      load-balance="True" 
      enable-client-logs="False" 
      log-level="error">
      <server 
        name="**.**.**.**"/>
    </cache>
  </configuration>
```

## Additional Resources

### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

### Programmers' Guide
The complete programmers guide for NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

## Technical Support

Alachisoft &copy; provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email at [support@alachisoft.com](mailto:support@alachisoft.com).

## Copyrights

&copy; Copyright 2021 Alachisoft