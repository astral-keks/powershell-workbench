using AstralKeks.Workbench.Core.Data;
using AstralKeks.Workbench.PowerShell.Attributes;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsData.Edit, Noun.WBConfiguration)]
    public class EditConfigurationCmdlet : WorkbenchDynamicPSCmdlet
    {
        private const string Userspace = "Userspace";
        private const string Workspace = "Workspace";

        [DynamicParameter(Position = 0)]
        [ValidateDynamicSet(nameof(GetLocations))]
        public string Location => Parameters.GetValue<string>(nameof(Location));

        [DynamicParameter(Position = 1)]
        [ValidateDynamicSet(nameof(GetConfigurations))]
        public string Configuration => Parameters.GetValue<string>(nameof(Configuration));

        protected override void ProcessRecord()
        {
            var rootDirectory = GetRootDirectory(Location);
            var configFile = ResolveConfigFile(Configuration);
            var configPath = Env.ConfigurationManager.GetConfigPath(rootDirectory, configFile);
            SessionState.InvokeCommand.InvokeScript($"'{configPath}' | {Application.Editor}");
        }

        public string[] GetLocations()
        {
            return new[] { Userspace, Workspace };
        }

        public string[] GetConfigurations()
        {
            return Env.ConfigurationManager.GetConfigFiles().Select(Path.GetFileNameWithoutExtension).ToArray();
        }

        private string GetRootDirectory(string location)
        {
            switch (location)
            {
                case Workspace:
                    return Env.WorkspaceManager.GetWorkspaceDirectory();
                case Userspace:
                    return Env.UserspaceManager.GetUserspaceDirectory();
                default:
                    throw new NotSupportedException(location);
            }
        }

        private string ResolveConfigFile(string configuration)
        {
            return Env.ConfigurationManager.GetConfigFiles().FirstOrDefault(f => f.StartsWith(configuration));
        }
    }
}
