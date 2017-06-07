using AstralKeks.Workbench.Core.Data;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsData.Edit, Noun.WBConfiguration)]
    public class EditConfigurationCmdlet : BaseConfigurationCmdlet
    {
        protected override void ProcessRecord()
        {
            var configPath = Env.ConfigurationManager.GetConfigPath(RootDirectory, ConfigFile);
            SessionState.InvokeCommand.InvokeScript($"'{configPath}' | {Application.Editor}");
        }
    }
}
