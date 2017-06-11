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
                CurrentWorkspaceDirectory = GetWorkspaceDirectory(Directory.GetCurrentDirectory());

            return CurrentWorkspaceDirectory;
        }

        public string GetWorkspaceDirectory(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Value is empty", nameof(directory));

            return _fileSystemManager.FindParentDirectory(directory, ExistsWorkspace);
        }

        public void SwitchWorkspace(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Value is empty", nameof(directory));

            directory = Path.GetFullPath(directory);
            var workspaceDirectory = GetWorkspaceDirectory(directory);

            CreateWorkspace(workspaceDirectory);

            CurrentWorkspaceDirectory = workspaceDirectory;
            Directory.SetCurrentDirectory(workspaceDirectory);
        }

        public void CreateWorkspace(string directory, string workspaceTemplate = null)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Value is empty", nameof(directory));
            workspaceTemplate = workspaceTemplate ?? Workspace.Default;

            var workspaceDirectory = Path.GetFullPath(directory);
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            var workspaceConfig = _configurationManager.GetWorkspaceConfig(userspaceDirectory);
            var workspace = workspaceConfig.FirstOrDefault(w => w.Name == workspaceTemplate);
            if (workspace == null)
                throw new ArgumentException($"Workspace template {workspaceTemplate} was not found");

            _fileSystemManager.CreateDirectoryIfNotExists(workspaceDirectory);

            foreach (var innerDirectory in workspace.Directories.Select(d => _fileSystemManager.GetAbsolutePath(workspaceDirectory, d)))
                _fileSystemManager.CreateDirectoryIfNotExists(innerDirectory);

            foreach (var file in workspace.Files)
            {
                var resourceDirectory = Path.GetDirectoryName(file);
                var resourceFilename = Path.GetFileName(file);
                _resourceManager.CreateResource(workspaceDirectory, userspaceDirectory, resourceDirectory, resourceFilename);
            }
            _resourceManager.CreateResource(workspaceDirectory, userspaceDirectory, 
                FileSystem.ConfigDirectory, FileSystem.WorkspaceMarkerFile);
        }

        private bool ExistsWorkspace(string directory)
        {
            var markerPath = _fileSystemManager.GetAbsolutePath(directory, FileSystem.ConfigDirectory, FileSystem.WorkspaceMarkerFile);
            return File.Exists(markerPath);
        }

        private string CurrentWorkspaceDirectory
        {
            get { return Environment.GetEnvironmentVariable("WBWorkspaceDirectory"); }
            set { Environment.SetEnvironmentVariable("WBWorkspaceDirectory", value); }
        }

    }
}

