<#
.SYNOPSIS
    Backs up or restores the data of an NCache cache (NCache 5.3).

.DESCRIPTION
    Backup  : Export-CacheData dumps every item in the cache to the backup folder.
    Restore : Import-CacheData reloads the items from the backup folder into the cache.

.EXAMPLE
    .\PersistenceBackup_Script.ps1 -CacheName demoCache -BackupPath "C:\CacheBackup" -Server 20.200.20.44

.EXAMPLE
    .\PersistenceBackup_Script.ps1 -CacheName demoCache -BackupPath "C:\CacheBackup" -Server 20.200.20.44 -Restore
#>
param(
    [Parameter(Mandatory = $true)] [string] $CacheName,
    [Parameter(Mandatory = $true)] [string] $BackupPath,
    [string] $Server = "localhost",
    [switch] $Restore,
    [ValidateSet("None", "Adjusted", "Asbefore")] [string] $Expiry = "Adjusted"
)

$ErrorActionPreference = "Stop"

try {
    # Load the NCache PowerShell module if it is not already loaded.
    if (-not (Get-Command Export-CacheData -ErrorAction SilentlyContinue)) {
        $module = Join-Path $env:NCHOME "bin\tools\ncacheps\ncacheps.psd1"
        if (-not $env:NCHOME -or -not (Test-Path $module)) {
            $module = "C:\Program Files\NCache\bin\tools\ncacheps\ncacheps.psd1"
        }
        Import-Module $module
    }

    # The cache must be registered and running on the server.
    # Get-Caches returns text, so read the "Status:" line of this cache's block.
    $caches = Get-Caches -Server $Server -Detail 6>&1 | Out-String
    $match = [regex]::Match($caches, "(?ims)^Cache-Name:\s*$([regex]::Escape($CacheName))\s*$.*?^Status:\s*(\w+)")
    if (-not $match.Success) {
        throw "Cache '$CacheName' is not registered on server $Server."
    }
    if ($match.Groups[1].Value -ne "Running") {
        throw "Cache '$CacheName' is not running on server $Server. Start it with: Start-Cache -Name $CacheName -Server $Server"
    }

    if ($Restore) {
        if (-not (Test-Path $BackupPath -PathType Container)) {
            throw "Backup folder '$BackupPath' not found."
        }
        Write-Host "Restoring '$CacheName' on $Server from $BackupPath ..."
        Import-CacheData -Name $CacheName -Path $BackupPath -Server $Server -Expiry $Expiry -NoLogo
        Write-Host "Restore completed." -ForegroundColor Green
    }
    else {
        if (-not (Test-Path $BackupPath)) {
            New-Item -Path $BackupPath -ItemType Directory | Out-Null
        }
        Write-Host "Backing up '$CacheName' on $Server to $BackupPath ..."
        Export-CacheData -Name $CacheName -Path $BackupPath -Server $Server -NoLogo
        Write-Host "Backup completed." -ForegroundColor Green
    }
}
catch [System.UnauthorizedAccessException] {
    Write-Host "Access denied to '$BackupPath'. Run with an account that has read/write rights on it." -ForegroundColor Red
    exit 1
}
catch {
    Write-Host $_ -ForegroundColor Red
    exit 1
}
