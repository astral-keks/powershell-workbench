using System;
using System.IO;
using AstralKeks.Workbench.Core.Data;
using System.Linq;

namespace AstralKeks.Workbench.Core.Management
{
    public class WorkspaceManager
    {
        private readonly FileSystemManager _fileSystemManager;
        private readonly ConfigurationManager _configurationManager;
        private readonly ResourceManager _resourceManager;

        public WorkspaceManager(ConfigurationManager configurationManager, FileSystemManager fileSystemManager,
            ResourceManager resourceManager)
        {
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
            _fileSystemManager = fileSystemManager ?? throw new ArgumentNullException(nameof(fileSystemManager));
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        }

        public string GetCurrentWorkspaceDirectory()
        {
            return GetWorkspaceDirectory(Directory.GetCurrentDirectory());
        }

        public Context SwitchWorkspace(string directory)
        {
            var currentWorkspace = GetWorkspaceDirectory(directory);
            return InitializeWorkspace(currentWorkspace).ApplyToEnvironment();
        }

        public Context InitializeWorkspace(string directory)
        {
            var workspace = _configurationManager.GetWorkspaceConfig();

            _fileSystemManager.CreateDirectoryIfNotExists(directory);

            foreach (var innerDirectory in workspace.Directories.Select(d => _fileSystemManager.GetAbsolutePath(directory, d)))
                _fileSystemManager.CreateDirectoryIfNotExists(innerDirectory);

            foreach (var file in workspace.Files)
                _resourceManager.InitializeResource(directory, file.Directory, file.Filename);
            _resourceManager.InitializeResource(directory, FileSystem.ConfigDirectory, FileSystem.WorkspaceMarkerFile);

            return new Context(directory);
        }

        private string GetWorkspaceDirectory(string directory)
        {
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));

            return _fileSystemManager.FindParentDirectory(directory, ExistsWorkspace);
        }

        private bool ExistsWorkspace(string directory)
        {
            var markerPath = _fileSystemManager.GetAbsolutePath(directory, FileSystem.ConfigDirectory, FileSystem.WorkspaceMarkerFile);
            return File.Exists(markerPath);
        }
    }
}

