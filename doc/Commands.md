# Initialize-WBEnvironment

## SYNOPSIS
Performs Workbench statup operations.

## SYNTAX

```
Initialize-WBEnvironment
```

## DESCRIPTION
This command creates aliases for applications, discovers toolkits and tries to find a workspace in any directory from the current up to the root. If no existing workspace was found a new one is created in the current directory. The output is list of paths to discovered toolkit modules. 

This command is executed by startup script right after PowerShell starts.

## EXAMPLES

### Example 1
```
PS C:\WorkpsaceDirectory\Some\Sub\Directory> Initialize-WBEnvironment
```

If `C:\WorkpsaceDirectory` is workspace it is used by Workbench. Otherwise a new one is created in `C:\WorkpsaceDirectory\Some\Sub\Directory`.


## PARAMETERS

### None

## INPUTS

### None

## OUTPUTS

### System.String

## NOTES

## RELATED LINKS


<br><br>
# Edit-WBConfiguration

## SYNOPSIS
Opens configuration file for editing.

## SYNTAX

```
Edit-WBConfiguration [[-Location] <String>] [[-Configuration] <String>]
```

## DESCRIPTION
This command uses Editor application to open configuration file specified in -Configuration parameter and stored in `Config` directory inside userspace or workspace (-Location parameter)

## EXAMPLES

### Example 1
```
PS C:\Workspace> Edit-WBConfiguration Workspace Application.json
```

This command opens `C:\Workspace\Config\Application.json`.

## PARAMETERS

### -Location
Configuration file location: userspace or workspace.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 
Accepted values: Userspace, Workspace

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Configuration
Name with extension of any file in userspace or workspace `Config` directory inside userspace or workspace: `Application.json`, `Workspace.json` etc.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None


## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS


<br><br>
# Reset-WBConfiguration

## SYNOPSIS
Reverts configuration file to its default state.

## SYNTAX

```
Reset-WBConfiguration [[-Location] <String>] [[-Configuration] <String>]
```

## DESCRIPTION
This command reverts configuration file specified in -Configuration parameter and stored in `Config` directory inside userspace or workspace (-Location parameter) to its default state.

## EXAMPLES

### Example 1
```
PS C:\> Reset-WBConfiguration Workspace Application.json
```

This command resets `C:\Workspace\Config\Application.json`.

## PARAMETERS

### -Location
Configuration file location: userspace or workspace.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 
Accepted values: Userspace, Workspace

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Configuration
Name with extension of any file located in `Config` directory inside userspace or workspace: `Application.json`, `Workspace.json` etc.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None


## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS


<br><br>
# New-WBWorkspace

## SYNOPSIS
Creates new workspace in specified directory using template from `Workspace.json`.

## SYNTAX

```
New-WBWorkspace [-Directory] <String> [[-Template] <String>]
```

## DESCRIPTION
This command creates new workspace in directory specified in -Directory parameter using template specified in -Template parameter. If no template is specified the one with name Default is used. Workspace templates are defined in `Workspace.json` inside userspace `Config` directory. 

## EXAMPLES

### Example 1
```
PS C:\> New-WBWorkspace D:\Some\Directory\Workspace1
```

Creates workspace in `D:\Some\Directory\Workspace1` using Default template.

### Example 2
```
PS C:\> New-WBWorkspace D:\Some\Directory\Workspace2 TemplateForMyWorkspaces
```

Creates workspace in `D:\Some\Directory\Workspace2` using TemplateForMyWorkspaces template.

## PARAMETERS

### -Directory
Directory to create workspace in.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Template
Template to create workspace from.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 
Accepted values: Default

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None


## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS


<br><br>
# Get-WBWorkspace

## SYNOPSIS
Gets workspace root directory.

## SYNTAX

```
Get-WBWorkspace [[-Directory] <String>]
```

## DESCRIPTION
This command gets workspace root searching for it in any directory from the one specified in -Directory parameter up to the root. If -Directory parameter is omitted the current directory is used as search starting point. If no existing workspace was found -Directory is returned.

## EXAMPLES

### Example 1
```
PS C:\> Get-WBWorkspace C:\WorkpsaceDirectory\Some\Sub\Directory
```

If `C:\WorkpsaceDirectory` is workspace the command returns it. Otherwise `C:\WorkpsaceDirectory\Some\Sub\Directory` is returned.

## PARAMETERS

### -Directory
Directory to start search from.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None


## OUTPUTS

### System.String

## NOTES

## RELATED LINKS


<br><br>
# Switch-WBWorkspace

## SYNOPSIS
Sets workspace current workspace.

## SYNTAX

```
Switch-WBWorkspace [-Directory] <String>
```

## DESCRIPTION
This command sets current workspace searching for it in any directory from the one specified in -Directory parameter up to the root. If no existing workspace was found a new one is created in -Directory.

## EXAMPLES

### Example 1
```
PS C:\> Switch-WBWorkspace C:\WorkpsaceDirectory\Some\Sub\Directory
```

If `C:\WorkpsaceDirectory` is workspace it is set as current. Otherwise a new one is created in `C:\WorkpsaceDirectory\Some\Sub\Directory`.

## PARAMETERS

### -Directory
Target workspace directory or sub-directory.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None


## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS


<br><br>
# Start-WBApplication

## SYNOPSIS
Starts application configured in `Application.json`

## SYNTAX

```
Start-WBApplication [-ApplicationName <String>] [[-CommandName] <String>]
 [[-Arguments] <System.Collections.Generic.List`1[System.String]>] [[-Pipeline] <String>]
```

## DESCRIPTION
This command starts application with name specified in -ApplicationName parameter using command (predefined arguments) specified in -CommandName parameter with arguments specified in -Arguments parameter and/or taking pipelined input as -Pipeline parameter. It can be invoked directly or using an alias created by Workbench from name of application described in `Application.json`. -ApplicationName parameter should be ommitted in that case (See examples).

## EXAMPLES

### Example 1
```
PS C:\> 'somevalue' | Start-WBApplication -ApplicationName Editor 
```

Starts Editor application using Default command.

### Example 2
```
PS C:\> 'somevalue' | Editor 
```

Equivalent to Example 1.

### Example 3
```
PS C:\> Terminal SomeCommand '.\Some\Path\For\Example'
```

Starts Terminal application using SomeCommand command.

## PARAMETERS

### -ApplicationName
Name of application described in `Application.json`

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CommandName
Name of command of spcified application described in `Application.json`

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Arguments
Argument list mapped to {$Args} variable in `Application.json`

```yaml
Type: System.Collections.Generic.List`1[System.String]
Parameter Sets: (All)
Aliases: 

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Pipeline
Pipeline value mapped to {$Pipeline} variable in `Application.json`

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 2
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

## INPUTS

### System.String


## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

