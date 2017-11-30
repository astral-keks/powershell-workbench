using System.IO;
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
        
        protected override void ProcessRecord()
        {
            if (!Components.WorkspaceController.CheckWorkspace(Directory, ShouldCreateWorkspace))
                throw new DirectoryNotFoundException($"Workspace does not exist in {Directory}");
            
            SessionState.Restore(() => Components.WorkspaceController.ExitWorkspace());
            SessionState.Update(() => Components.WorkspaceController.EnterWorkspace(Directory));
        }

        private bool ShouldCreateWorkspace()
        {
            return Force || ShouldContinue(
                $"Do you want to create workspace in {Directory}?",
                $"Workspace does not exist in {Directory}.");
        }
    }
}
    