
Get-WBApplication | foreach { Set-Alias $_.Name Start-WBApplication -Scope Global }