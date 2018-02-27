$version = "0.6.0"

$src = "$PSScriptRoot\src"
$doc = "$PSScriptRoot\doc"
$artifact = "$PSScriptRoot\artifact"
$package = "$PSScriptRoot\package"

$bin = "$artifact\bin"
$nupkg = "$artifact\nupkg"
$tmp = "$artifact\tmp"

$module = "$bin\Workbench"

$manifest = "$module\Workbench.psd1"

# Build binaries
if (Test-Path $artifact) { Remove-Item $artifact -Recurse }
if (Test-Path $package) { Remove-Item $package -Recurse }
dotnet restore $src\Command\Workbench.Command.csproj
dotnet build $src\Command\Workbench.Command.csproj --configuration Release -o $module --version-suffix $version

# Fix manifest version
(Get-Content $manifest).replace("ModuleVersion = '0.0.0'", "ModuleVersion = '$version'") | Set-Content $manifest

# Build packages
dotnet pack $src\Common\Workbench.Common.csproj --configuration Release -o $nupkg --version-suffix $version

# Build help xml
# (Get-Content $doc\Commands.md -Raw) -split "<br><br>" |
#     Select-Object -Skip 1 |
#     ForEach-Object {
#         $_ = $_.Trim()
#         $name = $_.Substring(0, $_.IndexOf("`r`n")).Trim("#").Trim()
#         $content = 
#             "---`r`n" + 
#             "external help file: AstralKeks.Workbench.Command.dll-Help.xml`r`n" + 
#             "online version: `r`n" + 
#             "schema: 2.0.0`r`n" + 
#             "---`r`n`r`n" + 
#             $_.Trim() +
#             "`r`n`r`n"
#         New-Item $tmp\$name.md -Force -ItemType File -Value $content
#     }

# Import-Module platyPS
# New-ExternalHelp $tmp -OutputPath $module -Force

# if (Test-Path $tmp) { Remove-Item $tmp -Recurse }