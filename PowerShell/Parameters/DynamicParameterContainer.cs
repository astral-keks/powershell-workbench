using System.Management.Automation;

namespace AstralKeks.Workbench.PowerShell.Parameters
{
    public class DynamicParameterContainer : RuntimeDefinedParameterDictionary
    {
        public TParameter GetValue<TParameter>(string parameterName)
        {
            return ContainsKey(parameterName) ? (TParameter)this[parameterName]?.Value : default(TParameter);
        }
    }
}
