# Sync NCache with PostgreSQL using NotifyExtensibleDependency

### Table of contents

* [Introduction](#introduction)
* [Postgresql Asynchronous Notifications Support For NCache Synchronisation](#postgresql-asynchronous-notifications-support-for-ncache-synchronisation)
* [Sample Application Structure](#sample-application-structure)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

This project highlights the NCache **NotifyExtensibleDependency** feature and how it can be used to synchronize NCache with a PostgreSQL database. [**NotifyExtensibleDependency**](https://www.alachisoft.com/resources/docs/ncache/prog-guide/notification-extensible-dependency.html) is a cache synchronization strategy for tackling the stale data problem, to be discussed shortly. Its primary purpose is to give the solution architect the flexibility to integrate a **real-time customized** logic that monitors and processes datastore change notifications **directly** into the cache hosting processes running on the cache servers. 

In this scheme, the developer deploys a provider containing the datastore state-change monitoring and processing logic behind the depedency along with the lifecycle hooks into the NCache servers. The servers then invoke the dependency-related methods which encapsulate the custom logic and determine whether or not to remove the associated cached item. The main strengths of this feature are as follows:	

- **Stale Data Problem Mitigation**

  Although using NCache with PostgreSQL boosts application performance, there is one issue that needs to be kept in mind. When you start caching with a primary data store such as PostgreSQL, two copies of the same data will be created, one in the primary data store and the other in the cache. Any direct update to the database data could render the cache data stale. With **NotifyExtensibleDependency**, not only are we taking advantage of the increased read performance provided by NCache, but we can also make sure that stale data does not persist in the cache.

- **Native NCache API Support**

  With support built into the NCache core logic, **NotifyExtensibleDependency** extends the power NCache provides when it comes to **high availability**, **reliability** and **scalability** and adds the mechanisms which ensure that cached-data fully agrees with the primary datastore state at all times.

The following are the advantages of NCache together with this feature when used with PostgreSQL:

- **Faster Read Operations**

  Using NCache as your distributed caching solution, application performance is improved since it is an in-memory key-value store which greatly improves data read performance. 
  
- **Improved Scalability**

  Using **NotifyExtensibleDependency**, all the cache synchronization operations are handed over to the clustered cache itself, allowing the clients to focus on the core business logic. 
  
  Not only does this create a clean logical separation of concerns among the NCache client and servers but it also provides improved scalability of the overall system architecture since any increase in notifications load can easily be handled by scaling out the NCache cluster independently of the client-side infrastructure, an important implication for today's cloud-based microservices applications. 

### Postgresql Asynchronous Notifications Support For NCache Synchronisation

The PostgreSQL Notifications system is based on the *LISTEN/NOTIFY* mechanism and is similar to the Pub/Sub Model. 

The client (in our case NCache) registers with a specific channel by calling the [*LISTEN*](https://www.postgresql.org/docs/10/sql-listen.html) command within the [***NotifyExtensibleDependency***](https://www.alachisoft.com/resources/docs/ncache/dotnet-api-ref/Alachisoft.NCache.Runtime.Dependencies.NotifyExtensibleDependency.html) *Initialize* method and then the PostgreSQL connection is put in a wait state by invoking the [***NpgsqlConnection***](https://www.npgsql.org/doc/api/Npgsql.NpgsqlConnection.html) [*Wait*](https://www.npgsql.org/doc/api/Npgsql.NpgsqlConnection.html#Npgsql_NpgsqlConnection_Wait) call.

On the PostgreSQL database side, you need a trigger that invokes a trigger function. By specifying that the trigger should be called AFTER UPDATE and AFTER DELETE, the trigger function sends a JSON payload on a channel. 

Any clients LISTENING on the channel using the previous LISTEN command get the payload and processes it using the information given therein. In our case, we take the payload and if the data matches the information about the cached item the dependency is attached to, we invoke the *DependencyChanged* event handler.

### Sample Application Structure

A sample ***NotifyExtensibleDependency*** implementation given in the sample follows the approach given in the previous section and you can view the source code [here](./src/PostGreSQLNotificationDependency/PostGreSQLDependency.cs).

Also given in this project are the scripts for creating the [demo customers table](./Resources/CustomerTable.sql), the [trigger](./Resources/CustomerTableTrigger.sql) to invoke the trigger function for notification and the [trigger function](./Resources/CustomerTriggerFunction.sql) itself. 

Besides these, there are two console applications that reference the [***NotifyExtensibleDependency***](./src/PostGreSQLNotificationDependency/) .NET Standard 2.0 library. One is [NETConsoleUI](./src/NETConsoleUI/) which is a .NET Framework 4.7.2 application and the other is [NETCOREConsoleUI](./src/NETCOREConsoleUI/) which is a .NET Core 3.1 application.

These two console applications are identical in their source code and are meant to show that both a .NET Core client and a .NET framework client can make use of the same ***NotifyExtensibleDependency*** feature.

### Prerequisites

Before the sample application is executed make sure that:

- The latest version of NCache Enterprise is installed. The latest version can be gotten from [here](https://www.alachisoft.com/download-ncache.html). If you are using a Linux cache server, download the Enterprise .NET Core installation. For a Windows cache server, you can install either the .NET Framework or the .NET Core installation.

  **NOTE**: The rest of this article assumes you have installed the .NET Framework installation although the steps for the .NET Core application are exactly the same.
  
- Make sure the relevant ports are open to allow for full communication between NCache and the PostgreSQL Database.
This sample was tested with PostgreSQL version "10.15"

- In your PostgreSQL database, run the SQL ***CREATE*** scripts given in the [Resources](./Resources/) folder to create the *customers* table, the *customers* table trigger that will be invoking the Notifications trigger function, and the trigger function itself.

  **IMPORTANT**:Confirm that the trigger is enabled.

- In both the [*NETConsoleUI*](./src/NETConsoleUI) and [*NETCOREConsoleUI*](./src/NETCOREConsoleUI) applications, change the *Program.cs* source code by adding the connection string to the PostgreSQL database that contains the *customers* table and the associate trigger and trigger function as shown below:

  ```csharp

  // Beginning of Program.cs

  // Change the connection string to your PostGreSQL Database
    const string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=database";

  // Rest of code
  ```
   
   Furthermore, make sure the [*client.ncconf*](https://www.alachisoft.com/resources/docs/ncache/admin-guide/client-config.html) is configured with the IP address of one of the servers of your demo cache in the *server* attribute as shown below:

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
		connection-timeout="5" port="9800"/>
    <cache 
		id="democache" 
		client-cache-id="" 
		client-cache-syncmode="optimistic" 
		default-readthru-provider="" 
		default-writethru-provider="" 
		load-balance="True"
		enable-client-logs="False" 
		log-level="error">
      	<server 
			name="***.***.***.***"/>
    </cache>
  </configuration>

   ```

   After making the above mentioned changes, build the solution in ***Release*** mode to generate the assemblies.

- Create a cache named *democache* using the [*NCache Web Manager*](https://www.alachisoft.com/resources/docs/ncache/admin-guide/ncache-web-manager.html?tabs=windows%2Cwindows-start-manager) GUI. The steps for doing this are given [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/create-new-cache-cluster.html?tabs=windows#using-ncache-web-manager).

  **IMPORTANT**: Do not start the cache after creating it as we will be deploying the ***NotificyExtensibleDependency*** implementation dependencies as shown in the next step.

- Once *democache* is created, we need to deploy the assemblies needed to allow for the PostgreSQL notification logic to run on the server side onto *democache*.

  Since we are assuming an NCache Enterprise .NET Framework server installation, we will deploying the .NET Framework assemblies. For that, use the assemblies given in the /bin/release folder for the [*NETConsoleUI*](./src/NETConsoleUI/) application.

  Once the assemblies are deployed using the steps given [here](https://www.alachisoft.com/resources/docs/ncache/admin-guide/deploy-providers.html), start the cache.
		
- On the database side, run an ***INSERT*** script on the *customers* table to insert a row with *customerid* field set to *ALFKI*. We will applying ***UPDATE*** and ***DELETE*** calls on this field to demonstrate real-time cache-invalidation on the NCache side using the ***NotifyExtensibleDependency*** implementation deployed with the assemblies during the previous step.

### Build and Run the Sample
    
- Run the *NETConsoleUI* application. This is will insert a dummy cache item in the cache with dependency set on it. This dependency is an instance of the ***NotifyExtensibleDependency*** implementation as shown [here](./src/PostGreSQLNotificationDependency/PostGreSQLDependency.cs).

- In the [*Statistics*](https://www.alachisoft.com/resources/docs/ncache/admin-guide/browse-cache-statistics.html?tabs=windows) page of the NCache Web Manager for *democache*, the *Count* field should now be *1*, signifying that the item was successfully added along with the dependency on the PostgreSQL database.

- On the database side, run either an ***UPDATE*** or a ***DELETE*** command script on the *customers* table for row with *customerid* field set to *ALFKI*. Once the transaction is complete, NCache will receive a notification from PostgreSQL using ***NOTIFY*** command within the trigger function that we created.

  In turn, the cached item will be invalidated and removed from cache, as demonstrated by the *Count* field in the *NCache Web Manager* *Statistics* page for *democache* going back to *0*.

- Repeat the steps for the *NETCoreConsoleUI* application to confirm that the same behavior is observed when data is added from a .NET Core Client application onto an NCache Enterprise .NET Framework application.

### Additional Resources

##### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

##### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/prog-guide/

### Technical Support

Alachisoft &copy; provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

&copy; 2020 Alachisoft 
