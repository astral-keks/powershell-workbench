using AstralKeks.Workbench.PowerShell.Provider;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Collections.Generic;

namespace AstralKeks.SourceControl.Provider
{
    [CmdletProvider("SourceControl", ProviderCapabilities.None)]
    [OutputType(typeof(string), ProviderCmdlet = ProviderCmdlet.ClearItem)]
    public class SoureControlProvider : CmdletProviderBase
    {
        protected override IEnumerable<PSDriveInfo> OnInit()
        {
            yield return new PSDriveInfo("Src", ProviderInfo, "src:\\", "Source Control Drive", Credential);
        }

        protected override CmdletProviderItem OnItem(string path)
        {
            return new SourceControlItem(path);
        }
    }
}
