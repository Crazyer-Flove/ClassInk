param([string]$OutputName = 'ClassInk.exe')
$ErrorActionPreference = 'Stop'
$framework = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319'
$refs = @('System.dll','System.Core.dll','System.Xml.dll','System.Drawing.dll','System.Windows.Forms.dll','WPF\WindowsBase.dll','WPF\PresentationCore.dll','WPF\PresentationFramework.dll','System.Xaml.dll') | ForEach-Object { '/reference:' + (Join-Path $framework $_) }
New-Item -ItemType Directory -Force -Path "$PSScriptRoot\dist" | Out-Null
& "$framework\csc.exe" /nologo /target:winexe /platform:x64 /optimize+ /out:"$PSScriptRoot\dist\$OutputName" /win32manifest:"$PSScriptRoot\app.manifest" /win32icon:"$PSScriptRoot\ClassInk.ico" "/resource:$PSScriptRoot\LICENSE,ClassInk.LICENSE" "/resource:$PSScriptRoot\assets\support-1.jpg,ClassInk.Support1.jpg" "/resource:$PSScriptRoot\assets\support-2.jpg,ClassInk.Support2.jpg" $refs "$PSScriptRoot\ClassInk.cs"
if ($LASTEXITCODE -ne 0) { throw 'Build failed' }
Write-Host "Built dist\$OutputName"

