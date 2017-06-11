using AstralKeks.Workbench.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace AstralKeks.Workbench.Core
{
    public class WorkbenchHost
    {
        private readonly WorkbenchEnvironment _env = new WorkbenchEnvironment();

        public void InstallEnvironment()
        {
            _env.InstallationManager.InstallVariables();
            _env.InstallationManager.InstallConfiguration();
        }

        public void UninstallEnvironment()
        {
            _env.InstallationManager.UninstallVariables();
            _env.InstallationManager.UninstallConfiguration();
        }

        public void ResetEnvironment()
        {
            UninstallEnvironment();
            InstallEnvironment();
        }

        public void StartWorkspace(string applicationName, List<string> arguments)
        {
            _env.WorkspaceManager.SwitchWorkspace(Directory.GetCurrentDirectory());
            _env.ApplicationManager.StartApplication(applicationName, Command.Workspace, arguments);
        }

        public void CreateWorkspace()
        {
            _env.WorkspaceManager.CreateWorkspace(Directory.GetCurrentDirectory());
        }
    }
}
