using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.Workbench.Core
{
    public class WorkbenchHost
    {
        private readonly WorkbenchEnvironment _env = new WorkbenchEnvironment();

        public void StartDefaultApplication()
        {
            _env.WorkspaceManager.SwitchWorkspace(Directory.GetCurrentDirectory());
            _env.ApplicationManager.StartApplication();
        }

        public string InstallEnvironment()
        {
            var path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
            var binDirectory = _env.FileSystemManager.GetBinDirectoryPath();
            if (!path.Contains(binDirectory))
                Environment.SetEnvironmentVariable("Path", $"{path};{binDirectory}", EnvironmentVariableTarget.User);

            return binDirectory;
        }

        public void StartApplication(string applicationName, string commandName, List<string> arguments)
        {
            _env.WorkspaceManager.SwitchWorkspace(Directory.GetCurrentDirectory());
            _env.ApplicationManager.StartApplication(applicationName, commandName, arguments);
        }

        public void CreateWorkspace()
        {
            _env.WorkspaceManager.InitializeWorkspace(Directory.GetCurrentDirectory());
        }
    }
}
