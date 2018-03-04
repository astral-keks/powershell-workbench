# This script is executed every time any userspace is used

Get-WBApplication | foreach { Set-Alias $_.Name Start-WBApplication -Scope Global }

Set-Alias Shortcut Resolve-WBShortcut -Scope Global
Set-Alias Template Resolve-WBTemplate -Scope Global
Set-Alias Backup Backup-WBItem -Scope Global
Set-Alias Restore Restore-WBItem -Scope Global