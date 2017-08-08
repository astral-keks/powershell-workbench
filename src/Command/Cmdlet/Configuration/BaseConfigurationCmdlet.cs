using AstralKeks.Workbench.PowerShell.Attributes;
using System;
using System.Linq;

namespace AstralKeks.Workbench.Command
{
    public class BaseConfigurationCmdlet : WorkbenchDynamicPSCmdlet
    {
        protected const string Userspace = "Userspace";
        protected const string Workspace = "Workspace";

        [DynamicParameter(Position = 0)]
        [ValidateDynamicSet(nameof(GetLocations))]
        public string Location => Parameters.GetValue<string>(nameof(Location));

        [DynamicParameter(Position = 1)]
        [DynamicCompleter(nameof(GetConfigurations))]
        public string Configuration => Parameters.GetValue<string>(nameof(Configuration));

        public string[] GetLocations()
        {
            return new[] { Userspace, Workspace };
        }

        public string[] GetConfigurations(string word)
        {
            return Env.ConfigurationManager.GetConfigFiles(RootDirectory);
        }

        protected string RootDirectory
        {
            get
            {
                switch (Location)
                {
                    case Workspace:
                        return Env.WorkspaceManager.GetWorkspaceDirectory();
                    case Userspace:
                        return Env.UserspaceManager.GetUserspaceDirectory();
                    default:
                        throw new NotSupportedException(Location);
                }
            }
        }

        protected string ConfigFile
        {
            get
            {
                return Env.ConfigurationManager.GetConfigFiles(RootDirectory)
                    .FirstOrDefault(f => f.Contains(Configuration));
            }
        }
    }
}
