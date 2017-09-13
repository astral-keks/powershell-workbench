using AstralKeks.Workbench.Models;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Exit, Noun.WBWorkspace)]
    public class ExitWorkspaceCmdlet : WorkbenchPSCmdlet
    {
        private Userspace _userspace;

        protected override void BeginProcessing()
        {
            SessionState.Restore(() =>
            {
                Components.WorkspaceController.ExitWorkspace();
                _userspace = Components.UserspaceController.ExitUserspace();
            });
        }

        protected override void EndProcessing()
        {
            SessionState.Update(() =>
            {
                if (_userspace != null)
                {
                    var userspace = Components.UserspaceController.EnterUserspace(_userspace);
                    SessionState.Update(userspace.Profile);
                }
            });
        }
    }
}
