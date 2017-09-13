using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.New, Noun.WBToolkitProject)]
    public class NewToolkitProjectCmdlet : WorkbenchDynamicPSCmdlet
    {
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Author { get; set; }

        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Directory { get; set; }

        protected override void ProcessRecord()
        {
            Components.ToolkitСontroller.InitializeToolkitProject(Directory, Name, Author);
        }
    }
}
