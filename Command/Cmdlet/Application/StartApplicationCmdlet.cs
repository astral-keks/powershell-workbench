using AstralKeks.Workbench.PowerShell.Attributes;
using System;
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

        [DynamicParameter(Position = 0)]
        [DynamicCompleter(nameof(GetCommands))]
        public string CommandName => Parameters.GetValue<string>(nameof(CommandName));

        [DynamicParameter(Position = 1)]
        public List<string> Arguments => Parameters.GetValue<List<string>>(nameof(Arguments));

        [DynamicParameter(Position = 2, ValueFromPipeline = true)]
        public string Pipeline => Parameters.GetValue<string>(nameof(Pipeline));

        protected override void ProcessRecord()
        {
            Env.ApplicationManager.StartApplication(ApplicationName, CommandName, Arguments, Pipeline);
        }

        public string[] GetApplications(string applicationNamePart)
        {
            return Env.ApplicationManager.GetApplications()
                .Select(a => a.Name)
                .Where(a => a.StartsWith(applicationNamePart ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        public string[] GetCommands(string commandNamePart)
        {
            return !string.IsNullOrWhiteSpace(ApplicationName)
                ? Env.ApplicationManager.GetCommands(ApplicationName)
                    .Select(c => c.Name)
                    .Where(c => c.StartsWith(commandNamePart ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                    .ToArray()
                : new string[0];
        }
    }
}
