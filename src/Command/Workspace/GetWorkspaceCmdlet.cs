using AstralKeks.Workbench.Models;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Get, Noun.WBWorkspace)]
    [OutputType(new[] { typeof(Workspace) })]
    public class GetWorkspaceCmdlet : WorkbenchPSCmdlet
    {
        [Parameter(Position = 0)]
        public string Directory { get; }

        protected override void ProcessRecord()
        {
            var directories = SessionState.Path.GetResolvedProviderPathFromPSPath(Directory, out ProviderInfo provider);
            foreach (var directory in directories)
            {
                var workspace = Components.WorkspaceRepository.FindWorkspace(directory);
                WriteObject(workspace);
            }
        }
    }
}
