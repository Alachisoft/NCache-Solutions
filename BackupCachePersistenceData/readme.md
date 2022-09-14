# Backup and Restore NCache Persisted Data

## Introduction

Using a distributed cache with persistence, you can store data to a
persistence store. NCache enables flexible backup of the persisted data
while the cache is active, taking into account the necessity for
periodic backup of the persistence store due to maintenance or other
causes. Thus, you can store a copy of your data in the cache and restore
it in the event of a crisis.

-   As for backing up persisted data, the write operations on the
    persistent store during backup may cause data corruption, it is
    crucial to remember. You can pause and resume the persistence of
    NCache data to prevent this kind of corruption. For example, you may
    first pause the data persistence to the persistence store, backup
    the persistent store, and then resume the data persistence to the
    persistence store.

-   Whereas for restoring persisted data, you just need to stop the
    cache, copy the backup, and then resume the cache, you can quickly
    restore data from the backup.

## **Backup NCache Persisted Data**

You can back up the persistence store of your cache using
Suspend-NCacheDataPersistence and Resume-NCacheDataPersistence cmdlets
together. The following example demonstrates how to backup persistence
store of a Distributed Cache with Persistence named ClusteredCache
created with NCache Persistence.

For running the provided script through PowerShell command,

*\'.\\PersistenceBackup_Script.ps1\' \'ClusteredCache\'
\'\\\\20.200.20.29\\MyPersistenceFiles\\ClusteredCache_db\'
\'\\\\20.200.20.29\\MyPersistenceFiles\\ClusteredCache1_db\'*

Here, you need to provide three parameters in the command,

> Name of Distributed Cache with Persistence
>
> UNC path of configured store location of cache
>
> Backup folder path

Valid paths must be specified and read access permissions must be
provided for UNC path of configured store location of cache and write
access permissions must be provided for backup folder path.

Note: Make sure there are no configuration changes (node additions or
deletions, or joins or departures) that could cause state transfers on
the cache while your data persistence to the persistence store has been
paused.

## **Restore NCache Persisted Data**

Data from the backup can easily be restored at any moment to your cache.
The persistence store of ClusteredCache can be used to recover backup
data using the example that follows.

-   First, you need to stop ClusteredCache using Stop-Cache PowerShell cmdelt
    or NCache Web Manager.The following command stops the cache named 
    ClusteredCache on the specified server 20.200.20.39.
    Stop-Cache ClusteredCache -Server 20.200.20.39


-   *Next, you need to copy all the items from your backup folder to the
    configured store location (UNC path) of your cache. You can find the
    UNC path of the persistence store configured for your cache under
    store information in NCache Web Manager. You can copy backup from
    the backup path to the configured store location either manually or
    using PowerShell. The following PowerShell command copies all items
    from backup folder to the store location of ClusteredCache.*

> *Copy-Item -Path \"F:\\PersistentCacheBackup\" -Destination
> \"\\\\fileserver\\stores\\demoClusteredCache_db\" -Recurse*

-   Once it is copied, start ClusteredCache cache on all server nodes
    from NCache Web Manager or using Start-Cache PowerShell cmdlet.
