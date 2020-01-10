# Sync Azure Cosmos DB with NCache using NotifyExtensibleDependency

## Table of contents

* [Introduction](#introduction)
* [Prerequisites](#pre-requisites)
* [NCache Features Highlighted in Application](#ncache-features-highlighted-in-application)
* [Running the Application](#running-the-application)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

## Introduction

This project highlights the NCache **NotifyExtensibleDependency** feature and how it can be used to synchronize NCache with Cosmos DB SQL API collections. The following are the advantages of NCache together with this feature when used with Azure Cosmos DB:

- **Faster Read Operations**

  Using NCache as your distributed caching solution, application performance is improved since it is an in-memory key-value store which   greatly improves data read performance. Furthermore, with more data operations being serviced by the cache instead of the underlying     Cosmos DB database, the transaction costs incurred in terms of RU/s are greatly reduced as well. More information about using caching   with Cosmos DB can be found [here](https://www.alachisoft.com/blogs/how-to-use-caching-with-azure-cosmos-db/). 
  
- **Stale Data Problem Mitigation**

  Although using NCache with Cosmos DB boosts application performance, thee is one issue that needs to be kept in mind. When you start     caching with a primary data store such as Cosmos DB, two copies of the same data will be created, one in the primary data store and     the other in the cache. Any direct update to the database data could render the cache data stale. With **NotifyExtensibleDependency**,   not only are we taking advantage of the increased read performance provided by NCache, but we can also make sure that stale data does   not persist in the cache.
  
- **Improved Scalability**

  Using **NotifyExtensibleDependency**, all the cache synchronization operations are handed over to the clustered cache itself, allowing   the clients to focus on the core business logic. Not only does this create a clean logical separation of concerns among the NCache       client and servers but it also provides improved scalability of the overall system architecture since any increase in change feed       load can easily be handled by scaling out the NCache cluster instead of having to perform scale-up on the client-side hardware, an       important implication for today's cloud-based microservices applications.
  
More information regarding **NotifyExtensibleDependency** can be found [here](https://www.alachisoft.com/resources/docs/ncache/prog-guide/notification-extensible-dependency.html). With cache synchronization logic deployed to the cache servers using **NotifyExtensibleDependency**, the overall architecture including NCache and the [Cosmos DB Change Feed][https://docs.microsoft.com/en-us/azure/cosmos-db/change-feed] can visualized as in the figure given below. This diagram also highlights the use of a [read-through provider](https://www.alachisoft.com/resources/docs/ncache/prog-guide/read-through-caching.html) to allow for **auto-reloading** updated data into the cache:

![Architectural Diagram](./resources/architectural_diagram.png)
  

## Pre-requisites

  Before running the application, make sure the following requirements have been met:

  - Windows 10 64-bit development machines with [.NET Framework 4.7.2 runtime](https://dotnet.microsoft.com/download/dotnet-framework/net472) for hosting the NCache servers.
  - The latest [Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) and allow for [remote network access](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator#running-on-a-local-network).
  - Make sure to [export the SSL certificate](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator#running-on-a-local-network) of the Cosmos DB emulator on each of the NCache server nodes to allow for change feed processing to work on the server side         implemented logic.
  - An IDE to run the code such as [Visual Studio 2019](https://visualstudio.microsoft.com/).
  - The .NET Framework 4.7.2 SDK and Runtime environments have been installed to compile and package the application. Those can be installed from [here](https://dotnet.microsoft.com/download/dotnet-framework/net472).
  - **NCache 5.0 SP2 Enterprise edition** is installed on the cache servers. The installation files can be found [here](https://www.alachisoft.com/download-ncache.html).
  - Make sure to update the NCache server information in the [**client.ncconf**](https://www.alachisoft.com/resources/docs/ncache/admin-guide/client-config.html) files included in the [console application project](./src/NotifyExtensibleDependencyTesterUI).This server information includes the cache ID of the created cache and the IP address of atleast one of the servers.
- Once the application has been packaged, stop the demo cache if it is running and deploy the [NotifyExtensibleDependency](./src/CustomDependencyNotifyImpl) assemblies on the cache servers. The steps for doing this are shown [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/deploy-providers.html). 
- Before running the application, make sure all the required caches are running and the Cosmos DB emulator can be accessed from the NCache server machines.

## NCache Features Highlighted in Application

## Running the Application

## Additional Resources

### Documentation
The complete online documentation for NCache is available at:
[http://www.alachisoft.com/resources/docs/](http://www.alachisoft.com/resources/docs/)

### Programmers' Guide
The complete programmers guide of NCache is available at:
[http://www.alachisoft.com/resources/docs/ncache/prog-guide/](http://www.alachisoft.com/resources/docs/ncache/prog-guide/)

## Technical Support

Alachisoft [C] provides various sources of technical support. 

- Please refer to [Alachisoft Support](http://www.alachisoft.com/support.html) to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

[C] Copyright 2020 Alachisoft 
