# BACKING SOURCE USING [DAPPER](https://github.com/StackExchange/Dapper)

### Table of contents

* [Introduction](#introduction)
* [Prerequisites](#prerequisites)
* [Build and Run the sample](#build-and-run-the-sample)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction

- This sample explains how NCache keeps the cached items synchronized with the database, and how NCache silently loads the expired items from the 
	datasource using its [*ReadThru*](https://www.alachisoft.com/resources/docs/ncache/prog-guide/read-through-caching.html) feature and similarly silently updates items using its [*WriteThru*](https://www.alachisoft.com/resources/docs/ncache/prog-guide/write-through-caching.html) feature. 
	
	We are using the *Dapper* micro-ORM with the backing source providers to demonstrate the fact that you can just as easily architect NCache backing-source providers with 3rd-party libraries as you would a client application. 

	Although *Dapper* is a very high-performance micro-ORM in its own right, there are ways in which NCache can boost this performance even further in the following ways:

	- 	NCache is a distributed caching solution that can be *scaled out* to manage the request hits reaching the database. Thus, as the request load increases, you can simply add more servers to the cache to furnish this high load without having to *scale-up* your database.

	- 	Being a distributed cache, NCache also improves performance in cases where multiple instances of an application are running behind a load balancer e.g. server farms, containerized environments etc. which can share the contents of the cache. 
	
		In this case, if any of the client instances queries for results the first time, any subsequest requests using the other instances will get the results directly from the cache.

	- 	A main issue regarding caching that people have reservations with is the case of stale data residing in the cache. To mitigate this issue, the NCache backing source providers together with database dependency features ensure that the data in the cache will remain in sync with the database using the NCache resync feature. This can be demonstrated by running the given sample and performing the following steps:

		1. Acquire results from database and cache them in NCache.
		1. Execute *UPDATE*, *DELETE* commands on the database using another application or directly in the *SQL Server Management Studio*.
		1. Run the same results again and confirm that the data in the cache is in sync with database.  

		Whats more, all the operations to resync the cache contents with the database are done on server side without client intervention!

	

### Prerequisites

Before the sample application is executed make sure that:

- app.config have been changed according to the configurations. 
	- Set the number of rows to be added to the test table by setting the *noOfInstances* key in the [App.Config](./ConsoleUI/App.Config) *appSettings* section. 
    - You can set the *UseCache* in the [App.Config](./ConsoleUI/App.config) *appSettings* section to *true* to see NCache backing source feature in action or you can get the data directly from the database using *Dapper* by setting this key to *false*. This will help in showing the performance upgrade offered by NCache in terms of read-performance. 
    - Create a database in Sql Server and provide the connection string in the [App.Config](./ConsoleUI/App.config) for connection name *sql*.
    - Set the *cacheId* to the Id of your cache in the [App.Config](./ConsoleUI/App.config) *appSettings* section.

- Before running this sample make sure backing source is enabled and following providers are registered.
	- For Read Thru
		[ReadThruProvider](./DapperBackingSource/ReadThruProvider.cs)
	- For Write Thru
		[WriteThruProvider](./DapperBackingSource/WriteThruProvider.cs)
		
- To enable and register backing sources,
	- Start NCache Web Manager and create a clustered cache with the name specified in app.config ***but do not start the cache yet***. 
	- Now select the 'Backing Source' tab in the "Advanced Settings" of cache's details page. 
	- To enable Read Through Backing Source,
		- Click the checkbox labelled "Enable Read Through". Click on "Add Provider" button next next to this checkbox.
		- Provide a unique provider name ("SqlReadThruProvider", for example).
		- Click on "Browse" button for library field and select the *bin* sub-folder within the [DapperBackingSource](./DapperBackingSource/) folder. Select library *Alachisoft.NCache.Samples.Dapper.BackingSources.dll*.
		- Select class *Alachisoft.NCache.Samples.Dapper.BackingSources.ReadThruProvider* from the now populated drop down list.
		- Specify connection string as *connectionString* parameter for database that is specified in [App.Config](./ConsoleUI/App.config) *connectionStrings* section. 
	- Similarly, to enable Write Thru backing source, follow the same steps as above. Choose "Alachisoft.NCache.Samples.Dapper.BackingSources.WriteThruProvider" from the class drop down list.
	- Backing source provider files need to be deployed.
		- Click 'Deploy Backing Source Provider' to deploy backing source. 
		- Locate and select *Alachisoft.NCache.Samples.Dapper.BackingSources.dll*, *Alachisoft.NCache.Samples.Dapper.Models.dll* and *Dapper.dll* in the *bin* folder within the [BackingSourceProvider](./DapperBackingSource/ReadThruProvider/) folder.
	- Click 'Ok' and save changes.
	- Force refresh the NCache Web Manager page and then start the cache.

### Build and Run the Sample
    
- Run the sample application. This will re-create the *Customers* table by first dropping it if it exists in the database specified by the connection string given in the [App.Config](./ConsoleUI/App.Config) *ConnectionStrings* section and will enable the SQL Server Service Broker to enable [SQL dependency](https://www.alachisoft.com/resources/docs/ncache/prog-guide/sql-dependency.html). It will also seed the database with the number of items as dictated by the *noOfInstances* key in the [App.Config](./ConsoleUI/App.Config) *appSettings* section.
- Select any of the menu items in the menu shown when application is run and make changes (update, delete only) in the database external to the application to see that the cached items are up to date. New items inserted in cache do not invalidate data through database dependencies so you still need expirations. However, the NCache resync mechanism, upon expiration, will re-acquire fresh data including any new rows in the result set.
- You can inspect the performance comparison between direct database access and using NCache backing source providers by looking at the time in ms taken to complete the read operations which will be output to the console window.
- Once you are done with the application, select option *7* from the menu which will exit the menu application. If you restart the application, the table will be recreated and seeded with the same exact initial data.

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
