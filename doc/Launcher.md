# Workbench Launcher

This console application is used to perform several basic Workbench operations without starting PowerShell.

## Syntax
```
workbench [command] [verb] [arguments] [options]
```

## Commands
```
workbench
```
Searches for workspace or creates it using Default template and starts Terminal in current directory.
<br><br>
```
workbench environment install [options]
```
Adds Workbench directory to PATH variable.
<br><br>
```
workbench workspace create [options]
```
Creates workspace in current directory using Default template.
<br><br>
```
workbench application start [arguments] [options]
```
Searches for workspace or creates it using Default template and starts application in current directory.

Arguments:
- `[name]`     application name
- `[command]`  application command name
- `[args]`     application arguments


## Options

 - `-h|--help|-?` Show help information.
