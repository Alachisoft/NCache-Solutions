# Azure Infrastructure as Code Through ARM Template


## Table of Contents

-   [Introduction](#introduction)
-   [Prerequisites](#prerequisites)
-   [Deploy NCache in Azure](#deploy-ncache-in-azure)
-   [ARM Template Parameters](#arm-template-parameters)
-   [Additional Resources](#additional-resources)

## Introduction


NCache managed ARM template allows you to automatically provision resources on
Azure in a single click by providing your customized parameters or using the
default configurations. This deploys “N” number of NCache VMs in Azure and
creates a running cluster of “N” nodes of the provided cache name. This ARM
template picks the latest NCache image from Azure Marketplace for deployment.

## Prerequisites

The "NCacheConfiguration.ps1" file in this project must be uploaded as Blob. You
can follow this link to upload files on Azure Storage account as
Blob: <https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-portal>.

## Deploy NCache in Azure


Once you have provided the parameters to the NCache managed ARM template, you
simply need to execute the following script in Azure PowerShell and it will
create the caches, cache cluster and start it for you:

```powershell
New-AzResourceGroupDeployment -ResourceGroupName <resource-group-name>
-TemplateFile https://raw.githubusercontent.com/Alachisoft/NCache-Solutions/master/AzureInfrastructureAsCodeArmTemplate/NCacheArmTemplate/azuredeploy.json
-TemplateParameterFile
https://raw.githubusercontent.com/Alachisoft/NCache-Solutions/master/AzureInfrastructureAsCodeArmTemplate/NCacheArmTemplate/azuredeploy.parameters.json

```

## ARM Template Parameters


### CacheName


Name(s) of the cache(s) to be created.

-   Example of single cache: `cache1`

-   Example of multiple caches: `cache1,cache2`

### CacheTopology


Defines the caching topology of the cache which will be created.

-   Example of single cache topology: `PartitionedOfReplica`

-   Example of multiple caches topology: `Partitioned, PartitionedOfReplica`

### CacheSize

Size of the cache in MBs.

-   Example of single cache size: `1024`

-   Example of multiple caches size: `512,1024`

### NCacheVmCount

The number of instances of the NCache image on Azure Marketplace that you want
to deploy on Azure.

### FirstName

First name of the user.

### LastName

Last name of the user.

### Company

Company name of the user.

### EmailAddress

The email address of the user.

### Environment Name

Name of the user’s environment for NCache. This is used in license activation
processes.

### NumberOfClients


The maximum number of clients allowed to connect with each node. This is used in
license activation processes.

### LicenseKey

NCache license key obtained from NCache Support.

### ReplicationStrategy

Cache replication strategy. This is either `asynchronous` or `synchronous`.

### EvictionPolicy

Eviction policy of cache items. This can be Least Recently Used (`LRU`), Least Frequently Used (`LFU`), `Priority`.

### EvictionPercentage

Percentage of eviction from the cache.

### VirtualMachineNamePrefix

The prefix of NCache VM.

### VirtualMachineSize

The size of NCache VM.

### AdminUserName

Administrator Username of NCache VM.

### AdminPassword

Administrator Password of NCache VM.

### AddressPrefix

Address prefix of VNET.

### SubnetName

Subnet name in VNET.

### SubnetPrefix

Subnet address prefix in VNET.

### VirtualNetworkName

Name of the virtual network.

### NCacheClusterCreationScriptFileUri

URI of blob storage of "NCacheConfiguration.ps1" file as mentioned in
[pre-requisites](#prerequisites). This must be uploaded by the user before
deploying the template. This script performs the task of creating the NCache
cluster and activating licenses and is mandatory to upload.

## Additional Resources

### Documentation

The complete online documentation for NCache is available
at: <http://www.alachisoft.com/resources/docs/#ncache>.

### Programmers' Guide

The complete Programmer's Guide of NCache is available
at: <http://www.alachisoft.com/resources/docs/ncache/prog-guide/>.

### Technical Support

Alachisoft [C] provides various sources of technical support.

-   Please refer to <http://www.alachisoft.com/support.html> to select a support
    resource you find suitable for your issue.

-   To request additional features in the future, or if you notice any
    discrepancy regarding this document, please drop an email
    to <support@alachisoft.com>.

## Copyrights

[C] Copyright 2020 Alachisoft
