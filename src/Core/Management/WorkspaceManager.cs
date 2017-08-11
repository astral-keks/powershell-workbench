using System;
using System.IO;
using AstralKeks.Workbench.Core.Data;
using System.Linq;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.FileSystem;
using AstralKeks.Workbench.Common.Resources;
using AstralKeks.Workbench.Core.Resources;

namespace AstralKeks.Workbench.Core.Management
{
    public class WorkspaceManager
    {
        private readonly UserspaceManager _userspaceManager;
        private readonly ConfigurationManager _configurationManager;
        private readonly ResourceManager _resourceManager;

        public WorkspaceManager(UserspaceManager userspaceManager, ConfigurationManager configurationManager, 
            ResourceManager resourceManager)
        {
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
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

            SystemVariable.WorkspaceDirectory = FindWorkspaceDirectory(directory, true);
            Directory.SetCurrentDirectory(SystemVariable.WorkspaceDirectory);
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

            FsOperation.CreateDirectoryIfNotExists(workspaceDirectory);
            foreach (var innerDirectoryName in workspaceTemplate.Directories)
            {
                var innerDirectory = FsPath.Absolute(workspaceDirectory, innerDirectoryName);
                FsOperation.CreateDirectoryIfNotExists(innerDirectory);
            }
            foreach (var file in workspaceTemplate.Files)
            {
                var resourceDirectory = Path.GetDirectoryName(file);
                var resourceFilename = Path.GetFileName(file);
                _resourceManager.CreateResource(new[] { workspaceDirectory, userspaceDirectory }, resourceDirectory, resourceFilename);
            }
            _resourceManager.CreateResource(new[] { workspaceDirectory, userspaceDirectory }, Files.WorkspaceLauncher);
        }

        private string FindWorkspaceDirectory(string directory, bool throwOnMissing)
        {
            var workspaceDirectory = FsOperation.FindParentDirectory(directory, d =>
            {
                var markerPath = FsPath.Absolute(d, null, Files.WorkspaceLauncher);
                return File.Exists(markerPath);
            });
            if (workspaceDirectory == null && throwOnMissing)
                throw new ArgumentException("Workspace directory was not found");
            return workspaceDirectory;
        }
    }
}

