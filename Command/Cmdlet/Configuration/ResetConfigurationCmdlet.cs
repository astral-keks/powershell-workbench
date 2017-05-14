using System;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Reset, Noun.WBConfiguration)]
    public class ResetConfigurationCmdlet : BaseConfigurationCmdlet
    {
        protected override void ProcessRecord()
        {
            var workspaceDirectory = Env.WorkspaceManager.GetWorkspaceDirectory();
            var userspaceDirectory = Env.UserspaceManager.GetUserspaceDirectory();
            switch (Location)
            {
                case Workspace:
                    Env.ConfigurationManager.ResetConfig(workspaceDirectory, userspaceDirectory, ConfigFile);
                    break;
                case Userspace:
                    Env.ConfigurationManager.ResetConfig(userspaceDirectory, ConfigFile);
                    break;
                default:
                    throw new NotSupportedException(Location);
            }
        }
    }
}
