$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$source = Join-Path $scriptDir "BuildSmartVentilationValidationKit.cs"
$exe = Join-Path $scriptDir "BuildSmartVentilationValidationKit.exe"
$csc = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
$swApi = "C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\api\redist"

& $csc /nologo /platform:x64 /target:exe /out:$exe `
  "/r:$swApi\SolidWorks.Interop.sldworks.dll" `
  "/r:$swApi\SolidWorks.Interop.swconst.dll" `
  $source

& $exe
