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
                    Env.ConfigurationManager.DeleteConfig(workspaceDirectory, ConfigFile);
                    Env.ConfigurationManager.CreateConfig(workspaceDirectory, userspaceDirectory, ConfigFile);
                    break;
                case Userspace:
                    Env.ConfigurationManager.DeleteConfig(userspaceDirectory, ConfigFile);
                    Env.ConfigurationManager.CreateConfig(userspaceDirectory, ConfigFile);
                    break;
                default:
                    throw new NotSupportedException(Location);
            }
        }
    }
}
