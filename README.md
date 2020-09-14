# NCache Solutions

This repository contains solutions used to highlight how various business applications can harness the power of NCache to achieve improved performance. NCache has a lot of powerful features such as integrations for EF, EF Core, NHiberate etc. and can server as a messaging broker for asynchronous communications between services. Furthermore, it provides support for both .NET and Java clients, thus allowing for sharing cache data between Java and .NET applications seamlessly.

Following is a list of the solutions provided:

- [eShopOnContainers using NCache](./eShopOnContainers/README.md)

  This solution is an extension of the well-known eShopOnContainers GitHub repository that aims to showcase the various enterprise design patterns when architecting a microservices-based multi-container application. While maintaining the original features including code to integrate RabbitMQ and NServiceBus message broker, this solution also showcases how NCache can be used for Pub/Sub messaging as well as object caching.
  
- [Persistent Cache with NCache](./PersistentCache/README.md)
 
  By default, NCache stores data in-memory which allows for data retrieval speeds orders of magnitude more than data storage media such as hard disks. A flip side to this feature is that memory data is violatile and can be lost irreversibly that could cause a major hit to business operations. This solution demonstrates an NCache persistance provider that can be used to back-up cache data seamlessly behind the scenes without application developer intervention.
	
- [Fraud Detection using NCache](./FraudDetection/README.md)

  This solution demonstrates how NCache can be integrated into a fraud-detection system that provides real-time results. The fraud detection system uses the Pub/Sub messaging feature of NCache as well as object caching to greatly boost throughput and availability inherited from the distributed, in-memory architecture of NCache.
    
- [Live Scoreboard with NCache](./LiveScoreboard/README.md)

  This solution showcases how a live scoreboard application such as a streaming sports scoreboard service can harness the power of NCache to boost its performance in terms of scalability, throughput and availability.
  
- [Full Text Search With Distributed Lucene](./FullTextSearchWithDistributedLucene/README.md)

  This solution demonstrates the ease with which the NCache Lucene module can be used to integrate Lucene Full Text Search (FTS) engine into a ASP.NET Core based e-commerce web application.

- [Microservices with NCache Using Service Fabric](./NCacheServiceFabric/README.md)

  This solution revisits the [eShopOnContainersSolution](./eShopOnContainers/README.md) solution but uses Azure Service Fabric to port the microservices as Service Fabric Reliable Stateless Services. The solution also contains an Azure Resource Manager (ARM) template to deploy a Service Fabric cluster into an existing VNET in Azure for convenience.
  
- [Sync Cosmos DB with NCache Using NotifyExtensibleDependency](./CosmosNotifyExtensibleDependency/README.md)

  This solution demonstrates the use of NotifyExtensibleDependency to allow for cache synchronization mechanisms to be deployed to the servers and uses a case study the change feed feature of Cosmos DB.
  
- [Taxi Fare Prediction using ML.NET and NCache](./TaxiFarePrediction/readme.md)

  This solution demonstrates how taxi fares can be predicted using a retrainable ML.NET model. It also demonstrates how NCache can be incorporated with real-time machine learning scenarios for boosting performance of the overall system.

- [Backing Source With Dapper Using NCache](./BackingSourceWithDapperUsingNCache/README.md)

  This solution shows how Dapper can be utilized together with NCache backing source providers and database dependency features to provide lighting-fast access to up-to-date database content directly from NCache.
  
- [IdentityServer4 with NCache](./NCacheIdentityServer4/README.md)

  This solution shows how NCache can be used as both a caching layer on top of IdentityServer4 configuration and operational stores as well as a standalone in-memory storage of configuration and operational data such as information about clients, identity resources, api resources, persisted grants etc.
