using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using AstralKeks.Workbench.Models;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Find, Noun.WBShortcut)]
    [OutputType(new[] { typeof(Shortcut) })]
    public class FindShortcutCmdlet : WorkbenchPSCmdlet
    {
        private const int _shortcutCountThreshold = 10;

        [Parameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty, ArgumentCompleter(typeof(ParameterCompleter))]
        public string ShortcutQuery { get; set; }

        protected override void ProcessRecord()
        {
            var shortcuts = Components.ShortcutController.FindShortcut(ShortcutQuery);
            WriteObject(shortcuts, true);
        }

        [ParameterCompleter(nameof(ShortcutQuery))]
        public IEnumerable<string> CompleteQuery(string queryPart)
        {
            return Components.ShortcutController.FindShortcut(queryPart).Select(s => s.ToString());
        }
    }
}
