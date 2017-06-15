using System.Collections.Generic;
using System;
using System.Linq;
using AstralKeks.Workbench.Core.Data;
using System.IO;

namespace AstralKeks.Workbench.Core.Management
{
    public class ToolkitManager
    {
        private readonly WorkspaceManager _workspaceManager;
        private readonly UserspaceManager _userspaceManager;
        private readonly ConfigurationManager _configurationManager;
        private readonly FileSystemManager _fileSystemManager;
        private readonly ResourceManager _resourceManager;
        private readonly MacrosManager _macrosManager;

        public ToolkitManager(WorkspaceManager workspaceManager, UserspaceManager userspaceManager,
            ConfigurationManager configurationManager, FileSystemManager fileSystemManager,
            ResourceManager resourceManager, MacrosManager macrosManager)
        {
            _workspaceManager = workspaceManager ?? throw new ArgumentNullException(nameof(workspaceManager));
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
            _fileSystemManager = fileSystemManager ?? throw new ArgumentNullException(nameof(fileSystemManager));
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
            _macrosManager = macrosManager ?? throw new ArgumentNullException(nameof(macrosManager));
        }

        public ToolkitRepository[] GetToolkitRepositories()
        {
            var workspaceDirectory = _workspaceManager.GetWorkspaceDirectory();
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            var repositories = _configurationManager.GetRepositoryConfig(workspaceDirectory, userspaceDirectory);
            foreach (var repository in repositories)
                repository.Directory = _macrosManager.ResolveMacros(repository.Directory);
            return repositories;
        }

        public string ResolveToolkitModule(string moduleName, string moduleBase, ICollection<ToolkitRepository> repositories)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                throw new ArgumentException("Module name is not set", nameof(moduleName));
            if (string.IsNullOrWhiteSpace(moduleBase))
                throw new ArgumentException("Module base is not set", nameof(moduleBase));
            if (repositories == null)
                throw new ArgumentNullException(nameof(repositories));

            ToolkitRepository repository = null;
            if (!string.IsNullOrWhiteSpace(moduleName) && !string.IsNullOrWhiteSpace(moduleBase))
            {
                repository = repositories
                    .Where(r => !string.IsNullOrWhiteSpace(r.Directory))
                    .FirstOrDefault(r => moduleBase.StartsWith(r.Directory));
            }
            if (repository != null && repository.Modules != null && repository.Modules.Any())
            {
                if (repository.Modules.All(m => m != moduleName))
                    repository = null;
            }

            return repository != null ? moduleName : null;
        }

        public ToolkitProjectInfo GetToolkitProjectInfo(string directory, string toolkitName, string toolkitAuthor)
        {
            if (string.IsNullOrWhiteSpace(toolkitName))
                throw new ArgumentException("Toolkit name is not set", nameof(toolkitName));
            if (string.IsNullOrWhiteSpace(toolkitAuthor))
                throw new ArgumentException("Toolkit author is not set", nameof(toolkitAuthor));

            directory = _fileSystemManager.GetAbsolutePath(directory);
            var srcDirectory = _fileSystemManager.GetAbsolutePath(directory, FileSystem.SourceDirectory);
            var coreDirectory = _fileSystemManager.GetAbsolutePath(srcDirectory, FileSystem.CoreDirectory);
            var commandDirectory = _fileSystemManager.GetAbsolutePath(srcDirectory, FileSystem.CommandDirectory);

            var author = new string(toolkitAuthor.Where(char.IsLetterOrDigit).ToArray());
            var moduleName = $"{author}.{toolkitName}";
            var moduleVersion = "0.0.1";
            var targetFramework = "net462";

            return new ToolkitProjectInfo
            {
                GeneratedOn = DateTime.UtcNow.ToString("M/d/yyyy"),

                ModuleName = moduleName,
                ModuleVersion = moduleVersion,
                ModuleAuthor = toolkitAuthor,
                ModuleGuid = Guid.NewGuid().ToString("D"),
                ModuleCopyright = $"(c) {DateTime.UtcNow.Year} {toolkitAuthor}. All rights reserved.",

                TargetFramework = targetFramework,
                CoreProjectName = $"{toolkitName}.{FileSystem.CoreDirectory}",
                CoreAssemblyName = $"{author}.{toolkitName}.{FileSystem.CoreDirectory}",
                CommandProjectName = $"{toolkitName}.{FileSystem.CommandDirectory}",
                CommandAssemblyName = $"{author}.{toolkitName}.{FileSystem.CommandDirectory}",
                OutputDirectoryPart = $"{targetFramework}\\{moduleName}\\{moduleVersion}",
                
                RootDirectory = directory,
                SolutionDirectory = srcDirectory,
                SolutionFilename = $"{toolkitName}.sln",
                CoreProjectDirectory = coreDirectory,
                CoreProjectFilename = $"{toolkitName}.{FileSystem.CoreDirectory}.csproj",
                CommandProjectDirectory = commandDirectory,
                CommandProjectFilename = $"{toolkitName}.{FileSystem.CommandDirectory}.csproj",
                ManifestFilename = $"{moduleName}.psd1",
                GitignoreFilename = FileSystem.GitignoreFile,
                CmdletDirectory = _fileSystemManager.GetAbsolutePath(commandDirectory, FileSystem.CmdletDirectory),
                ChangeLogPath = _fileSystemManager.GetAbsolutePath(directory, FileSystem.ChangeLogFile),
                LicencePath = _fileSystemManager.GetAbsolutePath(directory, FileSystem.LicenceFile),
                ReadmePath = _fileSystemManager.GetAbsolutePath(directory, FileSystem.ReadmeFile),
            };
        }

        public void CreateToolkitProject(ToolkitProjectInfo projectInfo)
        {
            if (projectInfo == null)
                throw new ArgumentNullException(nameof(projectInfo));

            var slnRes = _resourceManager.GetResource(projectInfo.SolutionDirectory, projectInfo.SolutionFilename);
            var slnResContent = slnRes.Read<string>();
            slnResContent = _macrosManager.ResolveMacros(slnResContent, projectInfo);
            slnRes.Write(slnResContent);

            var coreRes = _resourceManager.GetResource(projectInfo.CoreProjectDirectory, projectInfo.CoreProjectFilename);
            var coreResContent = coreRes.Read<string>();
            coreResContent = _macrosManager.ResolveMacros(coreResContent, projectInfo);
            coreRes.Write(coreResContent);

            var commandRes = _resourceManager.GetResource(projectInfo.CommandProjectDirectory, projectInfo.CommandProjectFilename);
            var commandResContent = commandRes.Read<string>();
            commandResContent = _macrosManager.ResolveMacros(commandResContent, projectInfo);
            commandRes.Write(commandResContent);

            var manifestRes = _resourceManager.GetResource(projectInfo.CommandProjectDirectory, projectInfo.ManifestFilename);
            var manifestResContent = manifestRes.Read<string>();
            manifestResContent = _macrosManager.ResolveMacros(manifestResContent, projectInfo);
            manifestRes.Write(manifestResContent);

            _resourceManager.CreateResource(projectInfo.RootDirectory, projectInfo.GitignoreFilename);

            _fileSystemManager.CreateDirectoryIfNotExists(projectInfo.CmdletDirectory);
            _fileSystemManager.CreateFileIfNotExists(projectInfo.ChangeLogPath);
            _fileSystemManager.CreateFileIfNotExists(projectInfo.LicencePath);
            _fileSystemManager.CreateFileIfNotExists(projectInfo.ReadmePath);
        }
    }
}
