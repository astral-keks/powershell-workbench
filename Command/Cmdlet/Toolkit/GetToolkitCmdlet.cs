using AstralKeks.Workbench.Core.Data;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Get, Noun.WBToolkit)]
    [OutputType(typeof(Toolkit))]
    public class GetToolkitCmdlet : WorkbenchDynamicPSCmdlet
    {
        protected override void ProcessRecord()
        {
            var toolkitDirectories = Env.ToolkitManager.GetToolkitDirectories().ToList();
            var toolkits = InvokeCommand.InvokeScript("Get-Module -ListAvailable")
                .Select(o => Env.ToolkitManager.ResolveToolkit(
                    o.Properties.FirstOrDefault(p => p.Name == "Name")?.Value as string,
                    o.Properties.FirstOrDefault(p => p.Name == "ModuleBase")?.Value as string,
                    toolkitDirectories))
                .Where(t => t != null);
            WriteObject(toolkits, true);
        }
    }
}
