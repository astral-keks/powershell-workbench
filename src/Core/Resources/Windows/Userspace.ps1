
Get-WBApplication | foreach { Set-Alias $_.Name Start-WBApplication -Scope Global }

Set-Alias Shortcut Resolve-WBShortcut -Scope Global
Set-Alias Template Resolve-WBTemplate -Scope Global