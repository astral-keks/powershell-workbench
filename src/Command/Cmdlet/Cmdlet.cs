using AstralKeks.PowerShell.Common;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    public class WorkbenchCmdlet : Cmdlet
    {
        protected readonly ComponentContainer Components = new ComponentContainer();
    }

    public class WorkbenchPSCmdlet : PSCmdlet
    {
        protected readonly ComponentContainer Components = new ComponentContainer();
    }

    public class WorkbenchDynamicCmdlet : DynamicCmdlet
    {
        protected readonly ComponentContainer Components = new ComponentContainer();
    }

    public class WorkbenchDynamicPSCmdlet : DynamicPSCmdlet
    {
        protected readonly ComponentContainer Components = new ComponentContainer();
    }
}
