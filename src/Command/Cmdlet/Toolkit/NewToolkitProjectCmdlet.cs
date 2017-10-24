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
        public string ToolkitDirectory { get; set; }

        protected override void ProcessRecord()
        {
            Components.ToolkitСontroller.InitializeToolkitProject(ToolkitDirectory, ToolkitName, ToolkitAuthor);
        }
    }
}
