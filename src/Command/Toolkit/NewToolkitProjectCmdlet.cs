using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.New, Noun.WBToolkitProject)]
    public class NewToolkitProjectCmdlet : WorkbenchPSCmdlet
    {
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ToolkitName { get; set; }

        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ToolkitAuthor { get; set; }

        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string ToolkitProjectPath { get; set; }

        protected override void ProcessRecord()
        {
            var resolvedPaths = SessionState.Path.GetResolvedProviderPathFromPSPath(ToolkitProjectPath, out ProviderInfo provider);
            foreach (var resolvedPath in resolvedPaths)
            {
                Components.ToolkitСontroller.InitializeToolkitProject(resolvedPath, ToolkitName, ToolkitAuthor);
            }
        }
    }
}
