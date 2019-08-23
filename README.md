# NCache Solutions

This repository contains solutions used to highlight how various business applications can harness the power of NCache to achieve improved performance. NCache has a lot of powerful features such as integrations for EF, EF Core, NHiberate etc. and can server as a messaging broker for asynchronous communications between services. Furthermore, it provides support for both .NET and Java clients, thus allowing for sharing cache data between Java and .NET applications seamlessly.

Following is a list of the solutions provided:

- [eShopOnContainers using NCache](./eShopOnContainers/README.md)

  This solution is an extension of the well-known eShopOnContainers GitHub repository that aims to showcase the various enterprise design patterns when architecting a microservices-based multi-container application. While maintaining the original features including code to integrate RabbitMQ and NServiceBus message broker, this solution also showcases how NCache can be used for Pub/Sub messaging as well as object caching.
