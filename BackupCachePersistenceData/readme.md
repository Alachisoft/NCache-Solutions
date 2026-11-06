# Backup and Restore NCache Cache Data

## Introduction

This sample backs up the data of a running NCache cache to a folder and
restores it back into the cache, using the NCache 5.3 PowerShell cmdlets:

-   **Backup** uses `Export-CacheData`. It dumps every item currently in
    the cache to a local or network folder while the cache keeps running.
-   **Restore** uses `Import-CacheData`. It reloads the items from that
    folder into the cache.

> NCache 5.3 no longer has the `Suspend-NCacheDataPersistence` and
> `Resume-NCacheDataPersistence` cmdlets used by earlier versions of this
> sample, so the script now uses `Export-CacheData` and `Import-CacheData`.

## Prerequisites

-   NCache 5.3 installed on the machine running the script. The script
    loads the NCache PowerShell module (`ncacheps`) itself.
-   The cache must be registered and **running** on the server.
-   The account running the script needs write access to the backup folder
    (for backup) or read access to it (for restore).

## Parameters

| Parameter     | Required | Description                                                        |
|---------------|----------|--------------------------------------------------------------------|
| `-CacheName`  | Yes      | Name of the cache.                                                 |
| `-BackupPath` | Yes      | Backup folder. Created on backup if it does not exist.             |
| `-Server`     | No       | IP or name of a server running the cache. Default: `localhost`.    |
| `-Restore`    | No       | Restore from `-BackupPath` instead of taking a backup.             |
| `-Expiry`     | No       | Expiration on restore: `None`, `Adjusted` (default), `Asbefore`.   |

## Backup Cache Data

Open PowerShell in this folder and run:

```powershell
.\PersistenceBackup_Script.ps1 -CacheName demoCache -BackupPath "C:\CacheBackup" -Server [server-ip]
```

The backup is written to a subfolder named after the cache, for example
`C:\CacheBackup\democache`.

## Restore Cache Data

Make sure the cache is running, then run the script with `-Restore` and the
same `-BackupPath`:

```powershell
.\PersistenceBackup_Script.ps1 -CacheName demoCache -BackupPath "C:\CacheBackup" -Server [server-ip] -Restore
```

## Running the cmdlets directly

The script is a wrapper around these two cmdlets, which you can also run
yourself:

```powershell
Import-Module "C:\Program Files\NCache\bin\tools\ncacheps\ncacheps.psd1"
Export-CacheData -Name demoCache -Path "C:\CacheBackup" -Server [server-ip]
Import-CacheData -Name demoCache -Path "C:\CacheBackup" -Server [server-ip]
```
