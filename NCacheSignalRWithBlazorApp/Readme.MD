# Using NCache ASP.NET Core SignalR with Blazor WebAssembly

## Introduction to ASP.NET Core Blazor
Blazor is a framework for building interactive client-side web UI with .NET:

* Create rich interactive UIs using C# instead of JavaScript.
* Share server-side and client-side app logic written in .NET.
* Render the UI as HTML and CSS for wide browser support, including mobile browsers.
* Integrate with modern hosting platforms, such as Docker.
* Using .NET for client-side web development offers the following advantages:

Write code in C# instead of JavaScript.
* Leverage the existing .NET ecosystem of .NET libraries.
* Share app logic across server and client.
* Benefit from .NET's performance, reliability, and security.
* Stay productive with Visual Studio on Windows, Linux, and macOS.
* Build on a common set of languages, frameworks, and tools that are stable, feature-rich, and easy to use.

To learn more about ASP.NET Core Blazor see [here](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-3.1)

## Introduction 

This solutions contains the information that how you can integrate NCache SignalR in Blazor App. Following sample was taken from microsoft to incorporate NCache signalR

View sample implementation of Blazor by microsoft here(how to download)

Prerequisites
* Visual Studio 2019 16.6 or later with the ASP.NET and web development workload
* .NET Core 3.1 SDK or later

## Installing and configuring NCache
1. Download and install NCache from [here](https://www.alachisoft.com/download-ncache.html)
2. After installation create cache named "blazor". Learn how to create cache [here](https://www.alachisoft.com/resources/docs/ncache/getting-started-guide-windows/create-cache.html).
3. Start the cache named "blazor".

## Add NCache SignalR in Blazor server startup serrvices 

Following code snippet will configure NCache SignalR in blazor Server. 
1. Download and install NCache ASP.NET Core SignalR [Nuget](https://www.nuget.org/packages/AspNetCore.SignalR.NCache/) in "BlazorSignalRApp.Server" project.


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

## Run the app
1. Open the "BlazorSignalRApp.sln" solution. 
2. Set the "BlazorSignalRApp.Server" as startup project.
3. Press F5 to run the solution.

### Technology Used

* [NCache](http://www.alachisoft.com/ncache/) - Distributed Cache for .NET and SignalR backplane.

### Development Tools

* [Visual Studio 2019](https://visualstudio.microsoft.com/)- For creation of project and code

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