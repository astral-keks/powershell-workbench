using AstralKeks.PowerShell.Common.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsLifecycle.Start, Noun.WBApplication)]
    public class StartApplicationCmdlet : WorkbenchDynamicPSCmdlet
    {
        [DynamicParameter]
        [DynamicCompleter(nameof(GetApplications))]
        public string ApplicationName => Parameters.GetValue<string>(nameof(ApplicationName)) ?? MyInvocation.InvocationName;

        [DynamicParameter]
        [DynamicCompleter(nameof(GetCommands))]
        public string CommandName => Parameters.GetValue<string>(nameof(CommandName));

        [DynamicParameter(Position = 0)]
        public List<string> Arguments => Parameters.GetValue<List<string>>(nameof(Arguments));

        [DynamicParameter(ValueFromPipeline = true)]
        public string Pipeline => Parameters.GetValue<string>(nameof(Pipeline));

        protected override void ProcessRecord()
        {
            Components.ApplicationController.StartApplication(ApplicationName, CommandName, Arguments, Pipeline);
        }

        public IEnumerable<string> GetApplications(string applicationNamePart)
        {
            return Components.ApplicationRepository.GetApplications().Select(a => a.Name);
        }

        public IEnumerable<string> GetCommands(string commandNamePart)
        {
            return !string.IsNullOrWhiteSpace(ApplicationName)
                ? Components.ApplicationRepository.GetCommands(ApplicationName).Select(c => c.Name)
                : Enumerable.Empty<string>();
        }
    }
}
