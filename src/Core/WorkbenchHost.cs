﻿using AstralKeks.Workbench.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.Workbench.Core
{
    public class WorkbenchHost
    {
        private readonly WorkbenchEnvironment _env = new WorkbenchEnvironment();

        public void InstallEnvironment(Func<bool> prompt)
        {
            if (prompt())
            {
                _env.InstallationManager.InstallVariables();
                _env.InstallationManager.InstallConfiguration();
            }
        }

        public void UninstallEnvironment(Func<bool> prompt)
        {
            if (prompt())
            {
                _env.InstallationManager.UninstallVariables();
                _env.InstallationManager.UninstallConfiguration();
            }
        }

        public void ResetEnvironment(Func<bool> prompt)
        {
            if (prompt())
            {
                _env.InstallationManager.UninstallVariables();
                _env.InstallationManager.UninstallConfiguration();
                _env.InstallationManager.InstallVariables();
                _env.InstallationManager.InstallConfiguration(); 
            }
        }

        public void StartWorkspace(string applicationName, List<string> arguments, Func<string, bool> prompt)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            if (!_env.WorkspaceManager.ExistsWorkspace(currentDirectory) && prompt(currentDirectory))
                _env.WorkspaceManager.CreateWorkspace(currentDirectory);

            _env.WorkspaceManager.SwitchWorkspace(currentDirectory);
            _env.ApplicationManager.StartApplication(applicationName, Command.Workspace, arguments);
        }

        public void CreateWorkspace(Func<string, bool> prompt)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            if (prompt(currentDirectory))
                _env.WorkspaceManager.CreateWorkspace(currentDirectory);
        }
    }
}
