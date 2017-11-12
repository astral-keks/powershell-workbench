using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Exit, Noun.WBWorkspace)]
    public class ExitWorkspaceCmdlet : WorkbenchPSCmdlet
    {
        protected override void ProcessRecord()
        {
            SessionState.Restore(() => Components.WorkspaceController.ExitWorkspace());
        }
    }
}
