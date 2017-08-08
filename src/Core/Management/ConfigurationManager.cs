using AstralKeks.Workbench.Common.FileSystem;
using AstralKeks.Workbench.Common.Resources;
using AstralKeks.Workbench.Core.Data;
using AstralKeks.Workbench.Core.Resources;
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
            return GetConfig<Application[]>(workspaceDirectory, userspaceDirectory, Files.Application);
        }

        public ToolkitRepository[] GetRepositoryConfig(string workspaceDirectory, string userspaceDirectory)
        {
            return GetConfig<ToolkitRepository[]>(workspaceDirectory, userspaceDirectory, Files.Toolkit);
        }

        public WorkspaceTemplate[] GetWorkspaceConfig(string userspaceDirectory)
        {
            return GetConfig<WorkspaceTemplate[]>(userspaceDirectory, Files.Workspace);
        }

        public TConfig GetConfig<TConfig>(string workspaceDirectory, string userspaceDirectory, string configFileName)
        {
            var resource = _resourceManager.ObtainResource(workspaceDirectory, userspaceDirectory, Directories.Config, configFileName);
            return resource.Read<TConfig>();
        }

        public TConfig GetConfig<TConfig>(string userspaceDirectory, string configFileName)
        {
            var resource = _resourceManager.ObtainResource(userspaceDirectory, Directories.Config, configFileName);
            return resource.Read<TConfig>();
        }

        public void CreateConfig(string workspaceDirectory, string userspaceDirectory, string configFileName)
        {
            _resourceManager.ObtainResource(workspaceDirectory, userspaceDirectory, Directories.Config, configFileName);
        }
        
        public void CreateConfig(string userspaceDirectory, string configFileName)
        {
            _resourceManager.ObtainResource(userspaceDirectory, Directories.Config, configFileName);
        }

        public void DeleteConfig(string directory, string configFileName)
        {
            _resourceManager.DeleteResource(directory, Directories.Config, configFileName);
        }

        public string GetConfigPath(string directory, string configFileName)
        {
            return new ResourceLocator(directory, Directories.Config, configFileName).Path;
        }

        public string[] GetConfigFiles(string directory)
        {
            return FsOperation.GetFilesInDirectory(directory, Directories.Config)
                .Select(Path.GetFileName)
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .ToArray();
        }

        public string[] GetConfigFileNames()
        {
            return new[]
            {
                Files.Application,
                Files.Toolkit,
                Files.Workspace
            };
        }
    }
}
