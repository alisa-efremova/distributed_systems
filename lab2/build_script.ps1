$msbuild = (Join-Path $env:windir 'Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe')
$projDir = Get-Location
$solution = "$projDir\VowelCalc\PoemBeautifier.sln"
$tempDir = "$projDir\tmp"
$prodDir = "$projDir\Release"

#check directories
if (-Not (Test-Path $msbuild)) {
    Write-Error "Error: MSBUILD not found"
	exit
}

if (-Not (Test-Path $solution)) {
    Write-Error "Error: Solution not found"
	exit
}

#clear temp folder
If (Test-Path $tempDir){
	Remove-Item $tempDir/*
}

#build solution
$collectionOfArgs = @("$solution", "/p:OutputPath=$tempDir", "/p:Configuration=Release")
& $msbuild $collectionOfArgs

#close current applications
If (Test-Path $prodDir){
	$files = Get-ChildItem -path $prodDir -Recurse -Include *.exe
	foreach ($file in $files)
	{
		$process = Get-Process $file.BaseName -ErrorAction SilentlyContinue
		if ($process) {
			$process.CloseMainWindow()
			if (!$process.HasExited) {
				$process | Stop-Process -Force -ErrorAction Stop
			}
		}
	}
	Sleep 1
	Remove-Item $prodDir/* -Recurse
	Remove-Item $prodDir
}

#rename tmp folder
Rename-Item $tempDir $prodDir -ErrorAction Stop

#run all applications from $prodDir
$files = Get-ChildItem -path $prodDir -Recurse -Include *.exe
foreach ($file in $files)
{
    Start-Process $file.Fullname -ArgumentList "/s"
}