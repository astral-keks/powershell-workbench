using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsLifecycle.Start, Noun.WBApplication)]
    public class StartApplicationCmdlet : WorkbenchPSCmdlet
    {
        [Parameter]
        [ArgumentCompleter(typeof(ParameterCompleter))]
        public string ApplicationName { get; set; }

        [Parameter]
        [ArgumentCompleter(typeof(ParameterCompleter))]
        public string CommandName { get; set; }

        [Parameter(Position = 0)]
        public List<string> CommandArguments { get; set; }

        [Parameter(ValueFromPipeline = true)]
        public string CommandPipeline { get; set; }

        protected override void ProcessRecord()
        {
            Components.ApplicationController.StartApplication(
                ApplicationName ?? MyInvocation.InvocationName, CommandName, CommandArguments, CommandPipeline);
        }

        [ParameterCompleter(nameof(ApplicationName))]
        public IEnumerable<string> GetApplications(string applicationNamePart)
        {
            return Components.ApplicationRepository.GetApplications().Select(a => a.Name);
        }

        [ParameterCompleter(nameof(CommandName))]
        public IEnumerable<string> GetCommands(string commandNamePart)
        {
            return !string.IsNullOrWhiteSpace(ApplicationName)
                ? Components.ApplicationRepository.GetCommands(ApplicationName).Select(c => c.Name)
                : Enumerable.Empty<string>();
        }
    }
}
