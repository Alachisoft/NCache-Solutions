# Using NCache ASP.NET Core SignalR with Blazor WebAssembly

## Introduction to ASP.NET Core Blazor
Blazor is a framework used to build interactive client-side web UI with .NET and has the following properties:

* Creates rich interactive UIs using C# instead of JavaScript.
* Shares server-side and client-side application logic written in .NET.
* Renders the UI as HTML and CSS supporting a wide range of browsers, including mobile browsers.
* Integrates with modern hosting platforms, such as Docker.

Using .NET for client-side web development over JavaScript offers the following advantages:
* Leverage the existing .NET ecosystem of .NET libraries.
* Share application's logic across the server and the client.
* Benefit from .NET's performance, reliability, and security.
* Is compatible with Visual Studio on Windows, Linux, and macOS.
* Can be built on a common set of languages, frameworks, and tools that are stable, feature-rich, and easy to use.

Learn more about ASP.NET Core Blazor [here](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0)

## Introduction 

This solution contains information on how to integrate NCache SignalR with the Blazor application. The following sample is taken from Microsoft to incorporate NCache SignalR with it:

View sample implementation of Blazor by Microsoft [here](https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/tutorials/signalr-blazor-webassembly/samples/3.x/BlazorSignalRApp).

## Pre-requisites
* Visual Studio 2022 (17.0 or later) with the ASP.NET and Web development workload
* .NET 6 SDK
* NCache 5.3 (Enterprise) server, with a cache created and running
* [AspNetCore.SignalR.NCache](https://www.nuget.org/packages/AspNetCore.SignalR.NCache/) 5.3.800 NuGet package (already referenced by the "BlazorSignalRApp.Server" project)

## Installing and configuring NCache
1. Download and install NCache [here](https://www.alachisoft.com/download-ncache.html)
2. After installation, create a cache named "blazor". Learn how to create a cache [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-cache.html).
3. Start the cache named "blazor". See how to start the cache by following NCache documentation [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/start-cache.html) 

## Configure the Application

The settings are in `appsettings.json` of the "BlazorSignalRApp.Server" project. The cache name and application ID go in the `NCacheConfiguration` section, and the server and port of the cache go in the `ncache` section, which NCache reads itself. No `client.ncconf` file is required. Replace `{cache server IP}` with the IP address of your NCache server:

```json
"NCacheConfiguration": {
  "CacheName": "BlazorCache",
  "ApplicationID": "chat"
},
"ncache": {
  "BlazorCache": {
    "server": "{cache server IP}",
    "port": 9800
  }
}
```

* **CacheName**: Name of the running cache used as the SignalR backplane.
* **ApplicationID**: Unique identifier of the application; also used as the event key for SignalR messages.
* **ncache**: one entry per cache, keyed by the cache name, with the IP address (`server`) and client port (`port`, 9800 by default) of the NCache server.

The application throws an error at startup if *CacheName* or *ApplicationID* is missing.

## Add NCache SignalR in Blazor Server Startup Services 

Following code snippet configures NCache SignalR in Blazor Server (`Startup.cs`). The values are read from `appsettings.json`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    var ncacheSection = Configuration.GetSection("NCacheConfiguration");
    // CacheName and ApplicationID are read and validated here
    ...
    // Server and port are read by NCache from the "ncache" section of appsettings.json
    services.AddSignalR().AddNCache(ncacheOptions =>
    {
        ncacheOptions.CacheName = cacheName;
        ncacheOptions.ApplicationID = applicationId;
        ncacheOptions.EventKey = applicationId;
    });
    ...
}
```

## Run the Application
1. Open the "BlazorSignalRApp.sln" solution. 
2. Set the "BlazorSignalRApp.Server" as startup project.
3. Press F5 to run the solution.

### Technology Used

* [NCache](http://www.alachisoft.com/ncache/) - Distributed Cache for .NET and SignalR backplane.

### Development Tools

* [Visual Studio 2022](https://visualstudio.microsoft.com/) - For creation of project and code.

## Additional Resources

#### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

#### Programmers' Guide
The complete programmer's guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

## Technical Support

Alachisoft [C] provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email at [support@alachisoft.com](mailto:support@alachisoft.com).

## Copyrights

[C] Copyright 2020 Alachisoft