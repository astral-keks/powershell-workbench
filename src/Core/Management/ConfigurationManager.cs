using AstralKeks.Workbench.Core.Data;
using System;
using System.IO;
using System.Linq;

namespace AstralKeks.Workbench.Core.Management
{
    public class ConfigurationManager
    {
        private readonly ResourceManager _resourceManager;

        public ConfigurationManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        }

        public Application[] GetApplicationConfig(string workspaceDirectory, string userspaceDirectory)
        {
            return GetConfig<Application[]>(workspaceDirectory, userspaceDirectory, FileSystem.ApplicationFile);
        }

        public string GetApplicationConfigPath(string workspaceOrUserspaceDirectory)
        {
            return GetConfigPath(workspaceOrUserspaceDirectory, FileSystem.ApplicationFile);
        }

        public ToolkitRepository[] GetRepositoryConfig(string workspaceDirectory, string userspaceDirectory)
        {
            return GetConfig<ToolkitRepository[]>(workspaceDirectory, userspaceDirectory, FileSystem.ToolkitFile);
        }

        public string GetRepositoryConfigPath(string workspaceOrUserspaceDirectory)
        {
            return GetConfigPath(workspaceOrUserspaceDirectory, FileSystem.ToolkitFile);
        }

        public WorkspaceTemplate[] GetWorkspaceConfig(string userspaceDirectory)
        {
            return GetConfig<WorkspaceTemplate[]>(userspaceDirectory, FileSystem.WorkspaceFile);
        }

        public string GetWorkspaceConfigPath(string userspaceDirectory)
        {
            return GetConfigPath(userspaceDirectory, FileSystem.WorkspaceFile);
        }

        public TConfig GetConfig<TConfig>(string workspaceDirectory, string userspaceDirectory, string configFile)
        {
            var resource = _resourceManager.GetResource<TConfig>(workspaceDirectory, userspaceDirectory, FileSystem.ConfigDirectory,
                configFile);
            return resource.Read();
        }

        public TConfig GetConfig<TConfig>(string userspaceDirectory, string configFile)
        {
            var resource = _resourceManager.GetResource<TConfig>(userspaceDirectory, FileSystem.ConfigDirectory, configFile);
            return resource.Read();
        }

        public void CreateConfig(string workspaceDirectory, string userspaceDirectory, string configFile)
        {
            _resourceManager.CreateResource(workspaceDirectory, userspaceDirectory, FileSystem.ConfigDirectory, configFile);
        }
        
        public void CreateConfig(string userspaceDirectory, string configFile)
        {
            _resourceManager.CreateResource(userspaceDirectory, FileSystem.ConfigDirectory, configFile);
        }

        public void DeleteConfig(string workspaceOrUserspaceDirectory, string configFile)
        {
            _resourceManager.DeleteResource(workspaceOrUserspaceDirectory, FileSystem.ConfigDirectory, configFile);
        }

        public string GetConfigPath(string workspaceOrUserspaceDirectory, string configFile)
        {
            return _resourceManager.GetResourcePath(workspaceOrUserspaceDirectory, FileSystem.ConfigDirectory, configFile);
        }

        public string[] GetConfigFiles(string workspaceOrUserspaceDirectory)
        {
            return _resourceManager.GetResourcePaths(workspaceOrUserspaceDirectory, FileSystem.ConfigDirectory)
                .Select(Path.GetFileName)
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .ToArray();
        }

        public string[] GetConfigFiles()
        {
            return new[]
            {
                FileSystem.ApplicationFile,
                FileSystem.ToolkitFile,
                FileSystem.WorkspaceFile
            };
        }
    }
}
