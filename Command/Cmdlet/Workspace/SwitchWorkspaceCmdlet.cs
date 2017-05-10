using AstralKeks.Workbench.Core.Data;
using AstralKeks.Workbench.PowerShell.Attributes;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Switch, Noun.WBWorkspace)]
    public class SwitchWorkspaceCmdlet : WorkbenchDynamicPSCmdlet
    {
        [DynamicParameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Directory => Parameters.GetValue<string>(nameof(Directory));

        protected override void ProcessRecord()
        {
            Env.WorkspaceManager.SwitchWorkspace(Directory);
            SessionState.InvokeCommand.InvokeScript($"Set-Location '{Directory}'");
        }
    }
}
