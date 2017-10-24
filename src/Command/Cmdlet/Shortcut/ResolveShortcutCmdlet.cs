using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using AstralKeks.PowerShell.Common.UserInterface;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsDiagnostic.Resolve, Noun.WBShortcut)]
    [OutputType(typeof(string))]
    public class ResolveShortcutCmdlet : WorkbenchPSCmdlet
    {
        private const int _shortcutCountThreshold = 10;

        [Parameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty, ArgumentCompleter(typeof(ParameterCompleter))]
        public string ShortcutQuery { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        protected override void ProcessRecord()
        {
            var shortcuts = Components.ShortcutRepository.FindShortcut(ShortcutQuery).Select(s => s.Target).ToList();
            var shouldWriteOutput = Force || shortcuts.Count <= _shortcutCountThreshold;
            if (!shouldWriteOutput)
            {
                var answers = new[] { PromptAnswer.Yes, PromptAnswer.No };
                var result = Host.UI.PromptForAnswer($"Shortcuts found: {shortcuts.Count}", "Do you want to proceed?", answers);
                shouldWriteOutput = result.SelectedValue == PromptAnswer.Yes;
            }
            if (shouldWriteOutput)
                WriteObject(shortcuts, true);
        }

        [ParameterCompleter(nameof(ShortcutQuery))]
        public IEnumerable<string> CompleteQuery(string queryPart)
        {
            return Components.ShortcutRepository.FindShortcut(queryPart).Select(s => s.ToString());
        }
    }
}
