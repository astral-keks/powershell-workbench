$src = "$PSScriptRoot\src"
$doc = "$PSScriptRoot\doc"
$artifact = "$PSScriptRoot\artifact"
$package = "$PSScriptRoot\package"
$tmp = "$artifact\tmp"

# Build binaries
Remove-Item $artifact -Recurse
Remove-Item $package -Recurse
dotnet restore $src\Launcher\Workbench.Launcher.csproj
dotnet build $src\Launcher\Workbench.Launcher.csproj --configuration Release -o  $artifact
dotnet restore $src\Command\Workbench.Command.csproj
dotnet build $src\Command\Workbench.Command.csproj --configuration Release -o  $artifact

# Build help xml
(Get-Content $doc\Commands.md -Raw) -split "<br><br>" |
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
New-ExternalHelp $tmp -OutputPath $artifact -Force

Remove-Item $tmp -Recurse