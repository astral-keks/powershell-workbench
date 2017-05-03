using AstralKeks.Workbench.Core;
using AstralKeks.Workbench.PowerShell;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    public class WorkbenchCmdlet : Cmdlet
    {
        protected readonly WorkbenchEnvironment Env = new WorkbenchEnvironment();
    }

    public class WorkbenchPSCmdlet : PSCmdlet
    {
        protected readonly WorkbenchEnvironment Env = new WorkbenchEnvironment();
    }

    public class WorkbenchDynamicCmdlet : DynamicCmdlet
    {
        protected readonly WorkbenchEnvironment Env = new WorkbenchEnvironment();
    }

    public class WorkbenchDynamicPSCmdlet : DynamicPSCmdlet
    {
        protected readonly WorkbenchEnvironment Env = new WorkbenchEnvironment();
    }
}
