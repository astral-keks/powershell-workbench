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
}
