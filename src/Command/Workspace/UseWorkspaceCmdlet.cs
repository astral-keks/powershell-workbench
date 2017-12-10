using System.IO;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsOther.Use, Noun.WBWorkspace)]
    public class UseWorkspaceCmdlet : WorkbenchPSCmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Directory { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }
        
        protected override void ProcessRecord()
        {
            var directories = SessionState.Path.GetResolvedProviderPathFromPSPath(Directory, out ProviderInfo provider);
            foreach (var directory in directories)
            {
                if (!Components.WorkspaceController.CheckWorkspace(directory, ShouldCreateWorkspace))
                    throw new DirectoryNotFoundException($"Workspace does not exist in {directory}");

                SessionState.Update(() => Components.WorkspaceController.UseWorkspace(directory));
            }
        }

        private bool ShouldCreateWorkspace(string directory)
        {
            return Force || ShouldContinue(
                $"Do you want to create workspace in {directory}?",
                $"Workspace does not exist in {directory}.");
        }
    }
}
    