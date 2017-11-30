using AstralKeks.Workbench.Models;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Get, Noun.WBWorkspace)]
    [OutputType(typeof(Workspace))]
    public class GetWorkspaceCmdlet : WorkbenchCmdlet
    {
        [Parameter(Position = 0)]
        public string Directory { get; }

        protected override void ProcessRecord()
        {
            var workspace = Components.WorkspaceRepository.FindWorkspace(Directory);
            WriteObject(workspace);
        }
    }
}
