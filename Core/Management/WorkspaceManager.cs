using System;
using System.IO;
using AstralKeks.Workbench.Core.Data;
using System.Linq;

namespace AstralKeks.Workbench.Core.Management
{
    public class WorkspaceManager
    {
        private readonly UserspaceManager _userspaceManager;
        private readonly FileSystemManager _fileSystemManager;
        private readonly ConfigurationManager _configurationManager;
        private readonly ResourceManager _resourceManager;

        public WorkspaceManager(UserspaceManager userspaceManager, ConfigurationManager configurationManager, 
            FileSystemManager fileSystemManager, ResourceManager resourceManager)
        {
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
            _fileSystemManager = fileSystemManager ?? throw new ArgumentNullException(nameof(fileSystemManager));
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        }

        public string GetWorkspaceDirectory()
        {
            if (string.IsNullOrEmpty(CurrentWorkspaceDirectory))
                CurrentWorkspaceDirectory = FindWorkspaceDirectory(Directory.GetCurrentDirectory());

            return CurrentWorkspaceDirectory;
        }

        public void SwitchWorkspace(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Value is empty", nameof(directory));

            directory = Path.GetFullPath(directory);
            var workspaceDirectory = FindWorkspaceDirectory(directory);

            CreateWorkspace(workspaceDirectory);

            CurrentWorkspaceDirectory = workspaceDirectory;
            Directory.SetCurrentDirectory(workspaceDirectory);
        }

        public void CreateWorkspace(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Value is empty", nameof(directory));

            var workspaceDirectory = Path.GetFullPath(directory);
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            var workspace = _configurationManager.GetWorkspaceConfig(userspaceDirectory);

            _fileSystemManager.CreateDirectoryIfNotExists(workspaceDirectory);

            foreach (var innerDirectory in workspace.Directories.Select(d => _fileSystemManager.GetAbsolutePath(workspaceDirectory, d)))
                _fileSystemManager.CreateDirectoryIfNotExists(innerDirectory);

            foreach (var file in workspace.Files)
                _resourceManager.InitializeResource(workspaceDirectory, userspaceDirectory, file.Directory, file.Filename);
            _resourceManager.InitializeResource(workspaceDirectory, userspaceDirectory, 
                FileSystem.ConfigDirectory, FileSystem.WorkspaceMarkerFile);
        }

        private bool ExistsWorkspace(string directory)
        {
            var markerPath = _fileSystemManager.GetAbsolutePath(directory, FileSystem.ConfigDirectory, FileSystem.WorkspaceMarkerFile);
            return File.Exists(markerPath);
        }

        private string FindWorkspaceDirectory(string directory)
        {
            return _fileSystemManager.FindParentDirectory(directory, ExistsWorkspace);
        }

        private string CurrentWorkspaceDirectory
        {
            get { return Environment.GetEnvironmentVariable("WBWorkspaceDirectory"); }
            set { Environment.SetEnvironmentVariable("WBWorkspaceDirectory", value); }
        }

    }
}

