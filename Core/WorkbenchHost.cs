using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.Workbench.Core
{
    public class WorkbenchHost
    {
        private readonly string _defaultApplicationName = "WorkspaceTerminal";
        private readonly WorkbenchEnvironment _env = new WorkbenchEnvironment();

        public List<string> ListApplications()
        {
            return _env.ApplicationManager.GetApplications().Select(a => a.Name).ToList();
        }

        public void StartApplication(string applicationName, string commandName, List<string> arguments)
        {
            applicationName = applicationName ?? _defaultApplicationName;
            _env.WorkspaceManager.SwitchWorkspace(Directory.GetCurrentDirectory());
            _env.ApplicationManager.StartApplication(applicationName, commandName, arguments);
        }

        public void CreateWorkspace()
        {
            _env.WorkspaceManager.InitializeWorkspace(Directory.GetCurrentDirectory());
        }

        public string Install()
        {
            var path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
            var binDirectory = _env.FileSystemManager.GetBinDirectoryPath();
            if (!path.Contains(binDirectory))
                Environment.SetEnvironmentVariable("Path", $"{path};{binDirectory}", EnvironmentVariableTarget.User);

            return binDirectory;
        }
    }
}
