$artifact = "$PSScriptRoot\artifact"
$bin = "$artifact\bin"
$module = "$bin\Workbench"

$apiKey = Read-Host -Prompt 'Enter API key'
Publish-Module -Path $module -NuGetApiKey $apiKey