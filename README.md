# NCache Solutions

This repository contains solutions used to highlight how various business applications can harness the power of NCache to achieve improved performance. NCache has a lot of powerful features such as integrations for EF, EF Core, NHiberate etc. and can server as a messaging broker for asynchronous communications between services. Furthermore, it provides support for both .NET and Java clients, thus allowing for sharing cache data between Java and .NET applications seamlessly.

Following is a list of the solutions provided:

- [eShopOnContainers using NCache](./eShopOnContainers/README.md)

  This solution is an extension of the well-known eShopOnContainers GitHub repository that aims to showcase the various enterprise design patterns when architecting a microservices-based multi-container application. While maintaining the original features including code to integrate RabbitMQ and NServiceBus message broker, this solution also showcases how NCache can be used for Pub/Sub messaging as well as object caching.
  
- [Persistent Cache with NCache](./PersistentCache/README.md)
 
  By default, NCache stores data in-memory which allows for data retrieval speeds orders of magnitude more than data storage media such as hard disks. A flip side to this feature is that memory data is violatile and can be lost irreversibly that could cause a major hit to business operations. This solution demonstrates an NCache persistance provider that can be used to back-up cache data seamlessly behind the scenes without application developer intervention.
	
- [Fraud Detection using NCache](./FraudDetection/README.md)

  This solution demonstrates how NCache can be integrated into a fraud-detection system that provides real-time results. The fraud detection system uses the Pub/Sub messaging feature of NCache as well as object caching to greatly boost throughput and availability inherited from the distributed, in-memory architecture of NCache.
