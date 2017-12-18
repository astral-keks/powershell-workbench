## 0.5.8
### Added
- All environment variables can be used in templates and configurations
### Fixed
- #9 Change $User config variable target

## 0.5.7
### Removed
- Implicit call to Use-WBUserspace Default during module import

## 0.5.6
### Added
- Strong name for AstralKeks.Workbench.Common

## 0.5.5
### Changed
- NuGet dependencies update
### Fixed
- Incorrect shortcut resolution

## 0.5.4
### Added
- New profile types
- Paths resolution in cmdlets

## 0.5.3
### Changed
- Alternative Userspace directory in SharedContext

## 0.5.2
### Fixed
- SharedContext dependency was not registered in module
### Changed 
- Initial backup id in Userspace
### Added
- Manifest version automatic update during build

## 0.5.1
### Added
- SharedContext in Workbench.Common to be used by toolkits

## 0.5.0
### Removed
- Modifying session when switching userspace or entering/exiting workspace
### Added
- Backup support
- Shortcut query wildcard support

## 0.4.2
### Fixed
- Macro variables are not working in Shortcut.json
### Added
- Userspace and Workspace transparent validation
- Shortcut discovery parameter enabling/disabling recursive search
- Shortcut and Template aliases

## 0.4.1
### Fixed
- Bugs related to shortcuts, templates, toolkit projects
### Added
- Publishing scipts

## 0.4.0
### Added
- Shortcuts support
- Templates support

## 0.3.0
### Removed
- Workbench command line utility
- Workspace templates
- Aliases configuration
- Toolkits configuration
### Added
- Multiple userspaces support
- Userspace and workspace profiles
- Session saving/restoration during entering/exiting workspace or userspace
- Saving recent userspace/workspace paths
### Changed
- Userspace/workspace are no longer required for Workbench to work properly

## 0.2.4
### Added
- Loader.psm1 file in New-WBToolkitProject

## 0.2.3
### Fixed
- Proper binaries and nupkg versioning

## 0.2.2
### Changed
- AstralKeks.Workbench.PowerShell -> AstralKeks.PowerShell.Common (NuGet package)

## 0.2.1
### Changed
- AstralKeks.Workbench.Common NuGet package
- build.cmd can be executed from any directory

## 0.2.0
### Added
- PS Provider base classes
- Sandbox project

## 0.1.11
### Added
- Evironment variables management when starting application
### Changed
- Toolkit Command project template
### Fixed
- Incorrect behavior of Reset-WBConfiguration cmdlet

## 0.1.10
### Removed
- Switch-WBWorkspace cmdlet
### Changed
- ResourceManager interface
- Default workspace template

## 0.1.9
### Changed
- New toolkit project manifest filename is changed to Manifest.psd1

## 0.1.8
### Added
- Alias config
### Fixed
- Incorrect {$Bin} directory resolution
- Resource locating without subdirectory

## 0.1.7
### Added
- AstralKeks.Workbench.Common library
### Changed
- VS build output directory is separate for each project
- Configuration files names got WB- prefix

## 0.1.6
### Added
- New-WBToolkitProject cmdlet

## 0.1.5
### Added
- Prompts for user input in Launcher

## 0.1.4
### Removed
- Automatic workspace creation

## 0.1.3
### Removed
- .Workspace file from workspace Config directory
### Fixed
- Incorrect Edit-WBConfiguration autosuggest
- Incorrect Start-WBApplication application arguments resolution

## 0.1.2
### Changed
- Reworked Launcher command-line interface

## 0.1.0
### Added
- Documentation
- New directory structure
- Build scripts

## 0.0.7
### Added
- Module list in Toolkit.json

## 0.0.6
### Added
- Reset-WBConfiguration cmdlet
- Get-WBToolkit cmdlet
### Changed
- Minor refactoring

## 0.0.5
### Added
- Get-WBWorkspace cmdlet
- New-WBWorkspace cmdlet
- Switch-WBWorkspace cmdlet
- Workspace templates
### Changed
- Minor refactoring
### Removed
- $Workbench variable
- Edit-WBConfiguration aliases

## 0.0.4
### Added
- Edit-WBConfiguration cmdlet
### Changed
- Minor refactoring

## 0.0.3
### Added
- $Workbench variable
### Changed
- Minor refactoring

## 0.0.2
### Changed
- Launcher refactoring

## 0.0.1
### Added
- Initial commit