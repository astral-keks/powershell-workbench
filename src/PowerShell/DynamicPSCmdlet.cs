using System.Management.Automation;
using AstralKeks.Workbench.PowerShell.Parameters;

namespace AstralKeks.Workbench.PowerShell
{
    public class DynamicPSCmdlet : PSCmdlet, IDynamicParameters
    {
        public DynamicParameterContainer Parameters { get; } = new DynamicParameterContainer();

        public object GetDynamicParameters()
        {
            var builder = new DynamicParameterBuilder(this);
            builder.Build(Parameters);

            return Parameters;
        }
    }
}
