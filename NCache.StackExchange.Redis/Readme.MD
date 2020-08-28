# NCache.StackExchange.Redis (NCache wrapper of StackExchange.Redis)
## Introduction
This solution will allow you to use NCache using the StackExchange.Redis APIs. This is not a redis client rather this is an NCache .Net client with StackExchange.Redis API's. This will help those who want to use NCache with minimal code change if they are already familiar with StackExchange.Redis (.Net client of redis). 
# Technology
NCache.StackExchange.Redis assembly is build with target framework of ".Netstandard 2.0".

## No Major Code change
To use this wrapper a sample project "NCache.StackExchange.Redis.Sample" is also provided which will let you play with NCache even more quickly.

* The code which is previously using StackExchange.Redis to interact with redis servers just need to remove the StackExchange.Redis Nuget and Use NCache.StackExcange.Redis.
* Namespace change will be from "StackExcange.Redis" to "NCache.StackExcange.Redis"


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