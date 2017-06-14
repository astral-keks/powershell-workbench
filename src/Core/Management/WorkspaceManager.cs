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
            return FindWorkspaceDirectory(Directory.GetCurrentDirectory(), true);
        }

        public string GetWorkspaceDirectory(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Value is empty", nameof(directory));
            
            return FindWorkspaceDirectory(directory, true);
        }
        
        public bool ExistsWorkspace(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Value is empty", nameof(directory));

            return FindWorkspaceDirectory(directory, false) != null;
        }

        public void SwitchWorkspace(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Value is empty", nameof(directory));

            CurrentWorkspaceDirectory = FindWorkspaceDirectory(directory, true);
            Directory.SetCurrentDirectory(CurrentWorkspaceDirectory);
        }

        public void CreateWorkspace(string directory, string workspaceTemplateName = null)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Value is empty", nameof(directory));
            workspaceTemplateName = workspaceTemplateName ?? WorkspaceTemplate.Default;

            var workspaceDirectory = Path.GetFullPath(directory);
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            var workspaceConfig = _configurationManager.GetWorkspaceConfig(userspaceDirectory);
            var workspaceTemplate = workspaceConfig.FirstOrDefault(w => w.Name == workspaceTemplateName);
            if (workspaceTemplate == null)
                throw new ArgumentException($"Workspace template {workspaceTemplateName} was not found");

            _fileSystemManager.CreateDirectoryIfNotExists(workspaceDirectory);
            foreach (var innerDirectoryName in workspaceTemplate.Directories)
            {
                var innerDirectory = _fileSystemManager.GetAbsolutePath(workspaceDirectory, innerDirectoryName);
                _fileSystemManager.CreateDirectoryIfNotExists(innerDirectory);
            }
            foreach (var file in workspaceTemplate.Files)
            {
                var resourceDirectory = Path.GetDirectoryName(file);
                var resourceFilename = Path.GetFileName(file);
                _resourceManager.CreateResource(workspaceDirectory, userspaceDirectory, resourceDirectory, resourceFilename);
            }
            _resourceManager.CreateResource(workspaceDirectory, userspaceDirectory, null, FileSystem.WorkspaceLauncherFile);
        }

        private string FindWorkspaceDirectory(string directory, bool throwOnMissing)
        {
            var workspaceDirectory = _fileSystemManager.FindParentDirectory(directory, d =>
            {
                var markerPath = _fileSystemManager.GetAbsolutePath(d, null, FileSystem.WorkspaceLauncherFile);
                return File.Exists(markerPath);
            });
            if (workspaceDirectory == null && throwOnMissing)
                throw new ArgumentException("Workspace directory was not found");
            return workspaceDirectory;
        }

        private string CurrentWorkspaceDirectory
        {
            get { return Environment.GetEnvironmentVariable("WBWorkspaceDirectory"); }
            set { Environment.SetEnvironmentVariable("WBWorkspaceDirectory", value); }
        }
    }
}

