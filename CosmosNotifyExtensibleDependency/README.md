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
  
More information regarding **NotifyExtensibleDependency** can be found [here](https://www.alachisoft.com/resources/docs/ncache/prog-guide/notification-extensible-dependency.html).
  

## Pre-requisites

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
