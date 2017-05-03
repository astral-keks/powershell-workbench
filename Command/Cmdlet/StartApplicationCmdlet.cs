using AstralKeks.Workbench.Core;
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
        [DynamicParameter(Position = 0)]
        [ValidateDynamicSet(nameof(GetParameterValues))]
        public string ApplicationName => Parameters.GetValue<string>(nameof(ApplicationName)) ?? MyInvocation.InvocationName;

        [DynamicParameter(Position = 1)]
        [DynamicCompleter(nameof(CompleteParameter))]
        public string CommandName => Parameters.GetValue<string>(nameof(CommandName));

        [DynamicParameter(Position = 2, ValueFromPipeline = true)]
        public string Argument => Parameters.GetValue<string>(nameof(Argument));

        protected override void ProcessRecord()
        {
            Env.ApplicationManager.StartApplication(ApplicationName, CommandName, new List<string> { Argument });
        }

        public string[] GetParameterValues()
        {
            return Env.ApplicationManager.GetApplications()
                .Select(a => a.Name)
                .ToArray();
        }

        public string[] CompleteParameter(string wordToComplete)
        {
            return Env.ApplicationManager.GetCommands(ApplicationName)
                .Where(c => c.Name.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Name)
                .ToArray();
        }
    }
}
