﻿using AstralKeks.Workbench.Common.Infrastructure;
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

        public Workspace CreateWorkspace(string directory)
        {
            var workspace = GetWorkspace(directory);
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

                _resourceRepository.CreateResource(workspace.Profile, Files.WorkspacePs1);

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
                    var workspace = GetWorkspace(d);
                    return _fileSystem.FileExists(workspace.Profile);
                });
            }

            return GetWorkspace(workspaceDirectory);
        }

        private Workspace GetWorkspace(string workspaceDirectory)
        {
            Workspace workspace = null;

            if (!string.IsNullOrWhiteSpace(workspaceDirectory))
            {
                workspaceDirectory = _fileSystem.MakeAbsolute(workspaceDirectory);
                workspace = new Workspace(workspaceDirectory);
            }

            return workspace;
        }
    }
}
