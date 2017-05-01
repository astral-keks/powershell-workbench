
$Script:directory = (Get-Item $PSScriptRoot).Parent.FullName
$Script:module = [IO.Path]::Combine($directory, 'AstralKeks.Workbench.Command.dll')

$module | Import-Module
Initialize-Workbench | Import-Module