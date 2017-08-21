$src = "$PSScriptRoot\src"
$doc = "$PSScriptRoot\doc"
$artifact = "$PSScriptRoot\artifact"
$package = "$PSScriptRoot\package"

$bin = "$artifact\bin"
$nupkg = "$artifact\nupkg"
$tmp = "$artifact\tmp"

# Build binaries
if (Test-Path $artifact) { Remove-Item $artifact -Recurse }
if (Test-Path $package) { Remove-Item $package -Recurse }
dotnet restore $src\Launcher\Workbench.Launcher.csproj
dotnet build $src\Launcher\Workbench.Launcher.csproj --configuration Release -o $bin
dotnet restore $src\Command\Workbench.Command.csproj
dotnet build $src\Command\Workbench.Command.csproj --configuration Release -o $bin

# Build packages
dotnet pack $src\Common\Workbench.Common.csproj --configuration Release -o $nupkg

# Build help xml
(Get-Content $doc\Commands.md -Raw) -split "<br><br>" |
    Select-Object -Skip 1 |
    ForEach-Object {
        $_ = $_.Trim()
        $name = $_.Substring(0, $_.IndexOf("`r`n")).Trim("#").Trim()
        $content = 
            "---`r`n" + 
            "external help file: AstralKeks.Workbench.Command.dll-Help.xml`r`n" + 
            "online version: `r`n" + 
            "schema: 2.0.0`r`n" + 
            "---`r`n`r`n" + 
            $_.Trim() +
            "`r`n`r`n"
        New-Item $tmp\$name.md -Force -ItemType File -Value $content
    }

Import-Module platyPS
New-ExternalHelp $tmp -OutputPath $bin -Force

if (Test-Path $tmp) { Remove-Item $tmp -Recurse }