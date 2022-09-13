write-host("SCRIPT") 
try {
	$cacheName = $args[0]
	$dbPath = $args[1]
	$destinationPath = $args[2]
	Write-Output "Destination Path: $destinationPath"
	Write-Output "DB Path: $dbPath"

	if (-not ($dbPath | Test-Path)) {
		Write-Host "$dbPath not found. Please specify a valid file or folder path." -foregroundcolor red 
		throw [System.IO.FileNotFoundException]::new("Thrown a file not found exception")
	}
	elseIf(-not ($destinationPath | Test-Path)){
		Write-Host "$destinationPath not found. Please specify a valid file or folder path." -foregroundcolor red 
		throw [System.IO.FileNotFoundException]::new("Thrown a file not found exception")
	}
	elseIf (($destinationPath -contains "/") -OR ($dbPath -contains "/")) { 
		Write-Host "Invalid Provided Paths" -foregroundcolor red 
	}
	elseIf(!($AccessRule1 -match "^FullControl") -AND !($AccessRule2 -match "^FullControl")){
		throw [System.UnauthorizedAccessException]::new("Thrown a UnauthorizedAccessException")
	}
	
    $ACL1 = Get-ACL -Path $dbPath
	$AccessRule1 = $ACL1.Access[0].FileSystemRights.ToString()
	Write-Host $AccessRule
	Write-Output "DB Path: $AccessRule1"
	
	if($destinationPath -eq $null){
		$destinationPath = Read-Host -Prompt "Enter destination path where you want to save files"
		Write-Output "Your $destinationPath" -foregroundcolor blue 
		New-Item -Path $destinationPath -ItemType Directory
		Write-Output "Your $destinationPath" -foregroundcolor blue
	}
	If($destinationPath -match "_db$"){
		$dataPath2 ="\data"
		$destinationPath = join-path -path $destinationPath -childpath $dataPath2
		Write-Host $destinationPath
	}
	If($dbPath -match "_db$"){
		$dataPath ="\data"
		$dbPath = join-path -path $dbPath -childpath $dataPath
		Write-Host $dbPath
	}

	$ACL2 = Get-ACL -Path $destinationPath
	$AccessRule2 = $ACL2.Access[0].FileSystemRights.ToString()
	Write-Host $AccessRule2

	Suspend-NCacheDataPersistence -CacheName $cacheName
	Get-ChildItem -Path $dbPath -Recurse -Filter ce_* | Copy-Item -Destination $destinationPath
	Get-ChildItem -Path $dbPath -Recurse -Filter dse_* | Copy-Item -Destination $destinationPath
	Resume-NCacheDataPersistence -CacheName $cacheName
	
}
catch [System.Management.Automation.ItemNotFoundException] {
	"Item not found"
	Write-Host $_ -foregroundcolor red
}
catch [System.IO.FileNotFoundException]{
	Write-Host $_ -foregroundcolor red
}
catch [System.UnauthorizedAccessException]
{
    Write-Host "Required File/Folder System Rights are not available." -foregroundcolor red
}
catch [System.SystemException] {
	
	Write-Host $_ -foregroundcolor red
}
catch [System.Exception] {
	
	Write-Host $_ -foregroundcolor red
}
catch {
	Write-Host "An error occurred:" -foregroundcolor red
	Write-Host $_ -foregroundcolor red
}
pause

