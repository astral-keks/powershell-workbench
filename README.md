# Workbench for PowerShell

Workbench is a command line tool (launcher) and a set of PowerShell commands which allow users manage workspaces, run applications in a manner similar to PowerShell commands (e.g. using pipelined input) and automatically load PowerShell modules.

Under development!

## Motivation

As a developer sooner or later you may come up with an idea that 
- each your project should have its **workspace** (a folder with some fixed sub-folder structure) on the local drive.
- you should be able to run terminal, text editor and some other **applications** inside a workspace.
- if terminal supports extensions/modules/**toolkits** it should be able to preload some of them automatically on startup.

I understand that there already exists software which solves these problems one way or another. But I decided that I'm capable to create a bit more convenient tool (at least for myself).


## Getting Started

### Prerequisites

- Windows or ~~Linux~~ (will be supported later).
- .NET Framework >=4.6.2 or ~~.NET Core 2.0~~ (will be supported later).
- Windows PowerShell or ~~PowerShell Core~~ (will be supported later).

### Building

To build Workbench from sources run `build.cmd` on Windows or ~~`build.sh` on Linux~~ (will be supported later).
Build produces 2 directories nearby `src` directory:
- `artifacts` - contains Workbench binaries
- `packages` - contains NuGet packages restored during build

### Installation

- Download ~~zip~~ (the link will be provided when development is finished).
- Extract it to some folder.
- Run following command in this folder:
  ```
  workbench environment install
  ```
- To verify installation initialize new workspace and start PowerShell by executing following command in any empty directory:
  ```
  workbench workspace start
  ```
  All Workbench commands should be available in created PowerShell session.

### Configuration

Workbench creates several configuration files in following locations:
- `Config` sub-directory inside workspace directory. Configuration files in this directory are used by workbench commands.
- `Config` sub-directory inside userspace directory (`%LOCALAPPDATA%\.Workbench` on Windows and ~~`$HOME/.Workbench` on Linux~~). Configuration files in this directory are copied to workspace during its creation.

Those files are:
- `Application.json` - configuration for application commands.
- `Toolkit.json` - toolkits discovery configuration.
- `Workspace.json` (only in userspace) - workspace templates. 

Look through created configuration files on your local machine for more information.

## Reference

 - ### [Commands](doc/Commands.md)

 - ### [Launcher](doc/Launcher.md)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.