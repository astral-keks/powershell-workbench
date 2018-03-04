# This script is executed every time any workspace is used

Get-WBApplication | foreach { Set-Alias $_.Name Start-WBApplication -Scope Global }