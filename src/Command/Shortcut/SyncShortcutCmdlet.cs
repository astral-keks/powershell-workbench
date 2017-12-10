using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsData.Sync, Noun.WBShortcut)]
    public class SyncShortcutCmdlet : WorkbenchCmdlet
    {
        [Parameter]
        public SwitchParameter InWorkspace { get; set; }

        [Parameter]
        public SwitchParameter InUserspace { get; set; }
        
        protected override void ProcessRecord()
        {
            Components.ShortcutController.SynchronizeShortcuts(InWorkspace, InUserspace);
        }
    }
}
