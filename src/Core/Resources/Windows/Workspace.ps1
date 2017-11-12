function global:Update-WBWorkspaceSession
{
    Get-WBApplication | foreach { Set-Alias $_.Name Start-WBApplication -Scope Global }
}

function global:Restore-WBWorkspaceSession
{
}