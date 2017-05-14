using AstralKeks.Workbench.Core.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsData.Initialize, Noun.WBEnvironment)]
    [OutputType(typeof(string))]
    public class InitializeEnvironmentCmdlet : WorkbenchPSCmdlet
    {
        protected override void ProcessRecord()
        {
            InitializeWorkspace();
            InitializeAliases();
            WriteObject(InitializeToolkits(), true);
        }

        private void InitializeWorkspace()
        {
            Env.WorkspaceManager.SwitchWorkspace(Directory.GetCurrentDirectory());

            var workspaceDirectory = Env.WorkspaceManager.GetWorkspaceDirectory();
            InvokeCommand.InvokeScript($"Set-Location '{workspaceDirectory}'");
        }

        private void InitializeAliases()
        {
            var cmdletInfo = typeof(StartApplicationCmdlet)
                .GetCustomAttributes(typeof(CmdletAttribute), true)
                .Cast<CmdletAttribute>()
                .First();

            var applications = Env.ApplicationManager.GetApplications();
            foreach (var application in applications)
                InvokeCommand.InvokeScript($"Set-Alias {application.Name} {cmdletInfo.VerbName}-{cmdletInfo.NounName} -Scope Global");
        }

        private IEnumerable<Toolkit> InitializeToolkits()
        {
            var psModulePathDirectories = Env.ToolkitManager.GetToolkitDirectories();
            var psModulePath = string.Join(";", psModulePathDirectories.Where(p => !string.IsNullOrWhiteSpace(p)));
            var oldPsModulePath = SessionState.PSVariable.GetValue("env:PSModulePath");
            try
            {
                InvokeCommand.InvokeScript($"$env:PSModulePath = '{psModulePath}'");

                return InvokeCommand.InvokeScript("Get-Module -ListAvailable")
                    .Select(o => Env.ToolkitManager.ResolveToolkit(
                        o.Properties.FirstOrDefault(p => p.Name == "Name")?.Value as string,
                        o.Properties.FirstOrDefault(p => p.Name == "ModuleBase")?.Value as string,
                        psModulePathDirectories))
                    .Where(t => t != null);
            }
            finally
            {
                InvokeCommand.InvokeScript($"$env:PSModulePath = '{oldPsModulePath};{psModulePath}'");
            }
        }
    }
}
