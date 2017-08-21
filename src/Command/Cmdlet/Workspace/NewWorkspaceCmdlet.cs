using AstralKeks.PowerShell.Common.Attributes;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.New, Noun.WBWorkspace)]
    public class NewWorkspaceCmdlet : WorkbenchDynamicCmdlet
    {
        [DynamicParameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Directory => Parameters.GetValue<string>(nameof(Directory));

        [DynamicParameter(Position = 1)]
        [ValidateDynamicSet(nameof(GetWorkspaceTemplateNames))]
        public string Template => Parameters.GetValue<string>(nameof(Template));

        protected override void ProcessRecord()
        {
            Env.WorkspaceManager.CreateWorkspace(Directory, Template);
        }

        public string[] GetWorkspaceTemplateNames()
        {
            var userspaceDirectory = Env.UserspaceManager.GetUserspaceDirectory();
            return Env.ConfigurationManager.GetWorkspaceConfig(userspaceDirectory)
                .Select(w => w.Name)
                .ToArray();
        }
    }
}
