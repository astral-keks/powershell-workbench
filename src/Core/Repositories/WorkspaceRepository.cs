using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Resources;
using System;
using System.IO;

namespace AstralKeks.Workbench.Repositories
{
    public class WorkspaceRepository
    {
        private readonly FileSystem _fileSystem;
        private readonly ProfileRepository _profileRepository;
        private readonly ResourceRepository _resourceRepository;

        public WorkspaceRepository(FileSystem fileSystem, ProfileRepository profileRepository, 
            ResourceRepository resourceRepository)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public Workspace CreateWorkspace(string directory)
        {
            var workspace = DefineWorkspace(directory);
            return CreateWorkspace(workspace);
        }

        public Workspace CreateWorkspace(Workspace workspace)
        {
            if (workspace != null)
            {
                var workspaceDirectory = _fileSystem.MakeAbsolute(workspace.Directory);

                var configDirectory = PathBuilder.Combine(workspaceDirectory, Directories.Config, Directories.Workbench);
                
                _fileSystem.DirectoryCreate(workspaceDirectory);
                _fileSystem.DirectoryCreate(configDirectory);

                var workspaceProfilePath = _profileRepository.CurrentWorkspace(workspaceDirectory);
                _resourceRepository.CreateResource(workspaceProfilePath, Files.WorkspacePs1);

                var discoveryConfigPath = PathBuilder.Combine(configDirectory, Files.DiscoveryJson);
                _resourceRepository.CreateResource(discoveryConfigPath, Files.DiscoveryWSJson);

                var shortcutConfigPath = PathBuilder.Combine(configDirectory, Files.ShortcutJson);
                _resourceRepository.CreateResource(shortcutConfigPath, Files.ShortcutWSJson);

                var applicationConfigPath = PathBuilder.Combine(configDirectory, Files.ApplicationJson);
                _resourceRepository.CreateResource(applicationConfigPath, Files.ApplicationWSJson);

                var backupConfigPath = PathBuilder.Combine(configDirectory, Files.BackupJson);
                _resourceRepository.CreateResource(backupConfigPath, Files.BackupWSJson);
            }

            return workspace;
        }

        public Workspace FindWorkspace(string innerDirectory)
        {
            string workspaceDirectory = null;

            if (!string.IsNullOrWhiteSpace(innerDirectory))
            {
                innerDirectory = _fileSystem.MakeAbsolute(innerDirectory);
                workspaceDirectory = _fileSystem.FindParentDirectory(innerDirectory, d =>
                {
                    var workspace = DefineWorkspace(d);
                    return _fileSystem.FileExists(workspace.Marker);
                });
            }

            return DefineWorkspace(workspaceDirectory);
        }

        public Workspace DefineWorkspace(string workspaceDirectory)
        {
            Workspace workspace = null;

            if (!string.IsNullOrWhiteSpace(workspaceDirectory))
            {
                workspaceDirectory = _fileSystem.MakeAbsolute(workspaceDirectory);

                var profiles = new[]
                {
                    _profileRepository.AllWorkspaces(),
                    _profileRepository.Workspaces(),
                    _profileRepository.CurrentWorkspace(workspaceDirectory)
                };

                var marker = PathBuilder.Complete(workspaceDirectory, Files.WorkspacePs1);

                workspace = new Workspace(workspaceDirectory, marker, profiles);
            }

            return workspace;
        }
    }
}
