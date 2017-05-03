using System.Collections.Generic;
using System.IO;

namespace AstralKeks.Workbench.Core
{
    public class WorkbenchHost
    {
        private readonly WorkbenchEnvironment _env = new WorkbenchEnvironment();

        public string InstallEnvironment()
        {
            return _env.InstallationManager.PerformInstallation();
        }

        public void StartDefaultApplication()
        {
            _env.WorkspaceManager.SwitchWorkspace(Directory.GetCurrentDirectory());
            _env.ApplicationManager.StartApplication();
        }

        public void StartApplication(string applicationName, string commandName, List<string> arguments)
        {
            _env.WorkspaceManager.SwitchWorkspace(Directory.GetCurrentDirectory());
            _env.ApplicationManager.StartApplication(applicationName, commandName, arguments);
        }

        public void CreateWorkspace()
        {
            _env.WorkspaceManager.CreateWorkspace(Directory.GetCurrentDirectory());
        }
    }
}
