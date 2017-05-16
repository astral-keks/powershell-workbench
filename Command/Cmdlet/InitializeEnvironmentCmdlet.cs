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

        private IList<string> InitializeToolkits()
        {
            var toolkitRepositories = Env.ToolkitManager.GetToolkitRepositories();
            var directoryToolkitRepositories = toolkitRepositories
                .Where(t => !string.IsNullOrWhiteSpace(t.Directory) && (t.Modules == null || !t.Modules.Any()))
                .ToList();

            var modules = toolkitRepositories.
                SelectMany(r => r.Modules ?? new string[0])
                .ToList();

            var directories = toolkitRepositories
                .Select(r => r.Directory)
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .ToArray();
            var psModulePath = string.Join(";", directories.Where(p => !string.IsNullOrWhiteSpace(p)));
            var oldPsModulePath = SessionState.PSVariable.GetValue("env:PSModulePath");

            try
            {
                InvokeCommand.InvokeScript($"$env:PSModulePath = '{psModulePath}'");

                var resolvedModules = InvokeCommand.InvokeScript("Get-Module -ListAvailable")
                    .Select(o => Env.ToolkitManager.ResolveToolkitModule(
                        o.Properties.FirstOrDefault(p => p.Name == "Name")?.Value as string,
                        o.Properties.FirstOrDefault(p => p.Name == "ModuleBase")?.Value as string,
                        directoryToolkitRepositories))
                    .Where(t => t != null);

                modules.AddRange(resolvedModules);
            }
            finally
            {
                InvokeCommand.InvokeScript($"$env:PSModulePath = '{oldPsModulePath};{psModulePath}'");
            }

            return modules;
        }
    }
}
