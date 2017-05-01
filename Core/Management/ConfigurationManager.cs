using AstralKeks.Workbench.Core.Data;
using System;

namespace AstralKeks.Workbench.Core.Management
{
    public class ConfigurationManager
    {
        private readonly ResourceManager _resourceManager;

        public ConfigurationManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        }

        public Application[] GetApplicationConfig(string workspaceDirectory)
        {
            var resource = _resourceManager.GetResource<Application[]>(
                workspaceDirectory,
                FileSystem.ConfigDirectory,
                FileSystem.ApplicationFile);
            return resource.Read();
        }

        public Repository[] GetRepositoryConfig(string workspaceDirectory)
        {
            var resource = _resourceManager.GetResource<Repository[]>(
                workspaceDirectory,
                FileSystem.ConfigDirectory,
                FileSystem.ToolkitFile);
            return resource.Read();
        }

        public Workspace GetWorkspaceConfig()
        {
            var resource = _resourceManager.GetResource<Workspace>(
                FileSystem.ConfigDirectory,
                FileSystem.WorkspaceFile);
            return resource.Read();
        }
    }
}
