using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Resources;
using System;

namespace AstralKeks.Workbench.Repositories
{
    public class WorkspaceRepository
    {
        private readonly FileSystem _fileSystem;
        private readonly ResourceRepository _resourceRepository;

        public WorkspaceRepository(FileSystem fileSystem, ResourceRepository resourceRepository)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public Workspace CreateWorkspace(string directory = null)
        {
            var workspace = GetWorkspace(directory) ?? GetWorkspace(_fileSystem.DirectoryGetCurrent());
            return CreateWorkspace(workspace);
        }

        public Workspace CreateWorkspace(Workspace workspace)
        {
            if (workspace != null)
            {
                var configDirectory = PathBuilder.Combine(workspace.Directory, Directories.Config);
                
                _fileSystem.DirectoryCreate(workspace.Directory);
                _fileSystem.DirectoryCreate(configDirectory);

                _resourceRepository.CreateResource(workspace.Profile, Files.WorkspacePs1);

                var shortcutConfigPath = PathBuilder.Combine(configDirectory, Files.WBShortcutJson);
                _resourceRepository.CreateResource(shortcutConfigPath, Files.ShortcutWSJson);

                var applicationConfigPath = PathBuilder.Combine(configDirectory, Files.WBApplicationJson);
                _resourceRepository.CreateResource(applicationConfigPath, Files.ApplicationWSJson);
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
