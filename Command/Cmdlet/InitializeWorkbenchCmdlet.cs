using AstralKeks.Workbench.Core;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsData.Initialize, Noun.Workbench)]
    [OutputType(typeof(string))]
    public class InitializeWorkbenchCmdlet : PSCmdlet
    {
        private readonly WorkbenchEnvironment _env = new WorkbenchEnvironment();

        protected override void ProcessRecord()
        {
            var applications = _env.ApplicationManager.GetApplications();
            foreach (var application in applications)
                InvokeCommand.InvokeScript($"Set-Alias {application.Name} Start-WorkbenchApplication -Scope Global");

            var psModulePathDirectories = _env.ToolkitManager.GetToolkitDirectories().ToList();
            var psModulePath = string.Join(";", psModulePathDirectories.Where(p => !string.IsNullOrWhiteSpace(p)));
            var oldPsModulePath = SessionState.PSVariable.GetValue("env:PSModulePath");
            try
            {
                InvokeCommand.InvokeScript($"$env:PSModulePath = '{psModulePath}'");

                var modules = InvokeCommand.InvokeScript("Get-Module -ListAvailable")
                    .Select(o => _env.ToolkitManager.ResolveToolkitModule(
                        o.Properties.FirstOrDefault(p => p.Name == "Name")?.Value as string,
                        o.Properties.FirstOrDefault(p => p.Name == "ModuleBase")?.Value as string,
                        psModulePathDirectories))
                    .Where(tm => tm != null);

                WriteObject(modules, true);
            }
            finally
            {
                InvokeCommand.InvokeScript($"$env:PSModulePath = '{oldPsModulePath};{psModulePath}'");
            }
        }
    }
}
