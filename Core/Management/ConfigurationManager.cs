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

        public Application[] GetApplicationConfig(string workspaceDirectory, string userspaceDirectory)
        {
            var resource = _resourceManager.GetResource<Application[]>(
                workspaceDirectory,
                userspaceDirectory,
                FileSystem.ConfigDirectory,
                FileSystem.ApplicationFile);
            return resource.Read();
        }

        public Repository[] GetRepositoryConfig(string workspaceDirectory, string userspaceDirectory)
        {
            var resource = _resourceManager.GetResource<Repository[]>(
                workspaceDirectory,
                userspaceDirectory,
                FileSystem.ConfigDirectory,
                FileSystem.ToolkitFile);
            return resource.Read();
        }

        public Workspace GetWorkspaceConfig(string userspaceDirectory)
        {
            var resource = _resourceManager.GetResource<Workspace>(
                userspaceDirectory,
                FileSystem.ConfigDirectory,
                FileSystem.WorkspaceFile);
            return resource.Read();
        }
    }
}
