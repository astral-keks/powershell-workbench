using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Resources;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Resources;
using System;

namespace AstralKeks.Workbench.Repositories
{
    public class WorkspaceRepository
    {
        private readonly FileSystem _fileSystem;
        private readonly ResourceManager _resourceManager;

        public WorkspaceRepository(FileSystem fileSystem, ResourceManager resourceManager)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        }

        public Workspace CreateWorkspace(string directory = null)
        {
            var workspace = GetWorkspace(directory) ?? GetWorkspace(_fileSystem.GetCurrentDirectory());
            return CreateWorkspace(workspace);
        }

        public Workspace CreateWorkspace(Workspace workspace)
        {
            if (workspace != null)
            {
                var configDirectory = PathBuilder.Combine(workspace.Directory, Directories.Config);
                var applicationConfigPath = PathBuilder.Combine(configDirectory, Files.ApplicationWSJson);

                _fileSystem.DirectoryCreate(workspace.Directory);
                _fileSystem.DirectoryCreate(configDirectory);

                _resourceManager.CreateResource(new[] { workspace.Profile }, Files.WorkspacePs1);
                _resourceManager.CreateResource(new[] { applicationConfigPath }, Files.ApplicationWSJson);
            }

            return workspace;
        }

        public Workspace FindWorkspace(string innerDirectory)
        {
            innerDirectory = _fileSystem.MakeAbsolute(innerDirectory);
            var workspaceDirectory = _fileSystem.FindParentDirectory(innerDirectory, d =>
            {
                var profile = PathBuilder.Combine(d, Files.WorkspacePs1);
                return _fileSystem.FileExists(profile);
            });

            return GetWorkspace(workspaceDirectory);
        }

        public Workspace GetWorkspace(string workspaceDirectory)
        {
            Workspace workspace = null;

            if (!string.IsNullOrWhiteSpace(workspaceDirectory))
            {
                workspaceDirectory = _fileSystem.GetFullPath(workspaceDirectory);
                var workspaceProfile = PathBuilder.Combine(workspaceDirectory, Files.WorkspacePs1);
                workspace = new Workspace(workspaceDirectory, workspaceProfile);
            }

            return workspace;
        }
    }
}
