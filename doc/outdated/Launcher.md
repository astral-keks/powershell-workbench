# Workbench Launcher

This console application is used to perform several basic Workbench operations without starting PowerShell.

## Syntax
```
workbench command verb [arguments] [options]
```
## Options
 - `-h|--help|-?` Show help information.
 - `-q|--quiet|-?` Produce no prompts or output.

## Commands, verbs, arguments
```
workbench environment install
```
Installs Workbench environment variables and configurations.
<br><br>
```
workbench environment uninstall
```
Uninstalls Workbench environment variables and configurations.
<br><br>
```
workbench environment reset
```
Resets Workbench environment variables and configurations to defaults.
<br><br>
```
workbench workspace create
```
Creates workspace in current directory.
<br><br>
```
workbench workspace start [arguments]
```
Starts application (Terminal by default) in current directory.

Arguments:
- `[app]` application name. Terminal is used by default.
- `[args]` application arguments.