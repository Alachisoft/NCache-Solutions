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

Learn more about ASP.NET Core Blazor [here](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-3.1)

## Introduction 

This solution contains information on how to integrate NCache SignalR with the Blazor application. The following sample is taken from Microsoft to incorporate NCache SignalR with it:

View sample implementation of Blazor by Microsoft [here](https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/tutorials/signalr-blazor-webassembly/samples/3.x/BlazorSignalRApp).

## Pre-requisites
* Visual Studio 2019 16.6 or later with the ASP.NET and Web development workload
* .NET Core 3.1 SDK or later

## Installing and configuring NCache
1. Download and install NCache [here](https://www.alachisoft.com/download-ncache.html)
2. After installation, create a cache named "blazor". Learn how to create a cache [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-cache.html).
3. Start the cache named "blazor". See how to start the cache by following NCache documentation [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/start-cache.html) 

## Add NCache SignalR in Blazor Server Startup Services 

Following code snippet configures NCache SignalR in Blazor Server. Download and install NCache ASP.NET Core SignalR [NuGet](https://www.nuget.org/packages/AspNetCore.SignalR.NCache/) in "BlazorSignalRApp.Server" project.


```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSignalR().AddNCache(ncacheOptions =>
            {
                ncacheOptions.CacheName = "blazor";
                ncacheOptions.ApplicationID = "chat";
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

* [Visual Studio 2019](https://visualstudio.microsoft.com/) - For creation of project and code.

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