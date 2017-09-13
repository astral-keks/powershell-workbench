using AstralKeks.Workbench.Models;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Enter, Noun.WBWorkspace)]
    public class EnterWorkspaceCmdlet : WorkbenchPSCmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Directory { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        private Userspace _prevUserspace;
        private Workspace _prevWorkspace;

        protected override void BeginProcessing()
        {
            SessionState.Restore(() =>
            {
                _prevWorkspace = Components.WorkspaceController.ExitWorkspace();
                _prevUserspace = Components.UserspaceController.ExitUserspace();
            });
        }

        protected override void EndProcessing()
        {
            SessionState.Update(() =>
            {
                var userspace = Components.UserspaceController.EnterUserspace(_prevUserspace);
                SessionState.Update(userspace?.Profile);

                var workspace = Components.WorkspaceController.EnterWorkspace(Directory, () =>
                {
                    Workspace w = null;
                    if (Force || ShouldContinue($"Do you want to create workspace in {Directory}?", $"Workspace does not exist in {Directory}."))
                        w = Components.WorkspaceRepository.CreateWorkspace(Directory);
                    return w;
                },
                () => _prevWorkspace);
                SessionState.Update(workspace?.Profile, workspace?.Directory);
            });
        }
    }
}
    