using System.Collections.Generic;
using System;
using System.Linq;
using AstralKeks.Workbench.Core.Data;
using AstralKeks.Workbench.Common.FileSystem;
using AstralKeks.Workbench.Common.Resources;
using AstralKeks.Workbench.Core.Resources;

namespace AstralKeks.Workbench.Core.Management
{
    public class ToolkitManager
    {
        private readonly WorkspaceManager _workspaceManager;
        private readonly UserspaceManager _userspaceManager;
        private readonly ConfigurationManager _configurationManager;
        private readonly MacrosManager _macrosManager;
        private readonly ResourceManager _resourceManager;

        public ToolkitManager(WorkspaceManager workspaceManager, UserspaceManager userspaceManager,
            ConfigurationManager configurationManager, ResourceManager resourceManager, MacrosManager macrosManager)
        {
            _workspaceManager = workspaceManager ?? throw new ArgumentNullException(nameof(workspaceManager));
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
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
                    .FirstOrDefault(r => moduleBase.StartsWith(r.Directory, StringComparison.OrdinalIgnoreCase));
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

            directory = FsPath.Absolute(directory);
            var srcDirectory = FsPath.Absolute(directory, Directories.Source);
            var coreDirectory = FsPath.Absolute(srcDirectory, Directories.Core);
            var commandDirectory = FsPath.Absolute(srcDirectory, Directories.Command);

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
                CoreProjectName = $"{toolkitName}.{Directories.Core}",
                CoreAssemblyName = $"{author}.{toolkitName}.{Directories.Core}",
                CommandProjectName = $"{toolkitName}.{Directories.Command}",
                CommandAssemblyName = $"{author}.{toolkitName}.{Directories.Command}",
                OutputDirectoryPart = $"{targetFramework}\\{moduleName}\\{moduleVersion}",
                
                RootDirectory = directory,
                SolutionDirectory = srcDirectory,
                SolutionFilename = $"{toolkitName}.sln",
                CoreProjectDirectory = coreDirectory,
                CoreProjectFilename = $"{toolkitName}.{Directories.Core}.csproj",
                CommandProjectDirectory = commandDirectory,
                CommandProjectFilename = $"{toolkitName}.{Directories.Command}.csproj",
                LoaderSourceFilename = Files.Loader,
                ManifestSourceFilename = Files.Manifest,
                GitignoreFilename = Files.Gitignore,
                CmdletDirectory = FsPath.Absolute(commandDirectory, Directories.Cmdlet),
                ChangeLogPath = FsPath.Absolute(directory, Files.ChangeLog),
                LicencePath = FsPath.Absolute(directory, Files.Licence),
                ReadmePath = FsPath.Absolute(directory, Files.Readme),
            };
        }

        public void CreateToolkitProject(ToolkitProjectInfo projectInfo)
        {
            if (projectInfo == null)
                throw new ArgumentNullException(nameof(projectInfo));

            var slnRes = _resourceManager.CreateResource(projectInfo.SolutionDirectory, projectInfo.SolutionFilename);
            var slnResContent = slnRes.Read<string>();
            slnResContent = _macrosManager.ResolveMacros(slnResContent, projectInfo);
            slnRes.Write(slnResContent);

            var coreRes = _resourceManager.CreateResource(projectInfo.CoreProjectDirectory, projectInfo.CoreProjectFilename);
            var coreResContent = coreRes.Read<string>();
            coreResContent = _macrosManager.ResolveMacros(coreResContent, projectInfo);
            coreRes.Write(coreResContent);

            var commandRes = _resourceManager.CreateResource(projectInfo.CommandProjectDirectory, projectInfo.CommandProjectFilename);
            var commandResContent = commandRes.Read<string>();
            commandResContent = _macrosManager.ResolveMacros(commandResContent, projectInfo);
            commandRes.Write(commandResContent);

            var manifestRes = _resourceManager.CreateResource(projectInfo.CommandProjectDirectory, projectInfo.ManifestSourceFilename);
            var manifestResContent = manifestRes.Read<string>();
            manifestResContent = _macrosManager.ResolveMacros(manifestResContent, projectInfo);
            manifestRes.Write(manifestResContent);

            var loadertRes = _resourceManager.CreateResource(projectInfo.CommandProjectDirectory, projectInfo.LoaderSourceFilename);
            var loadertResContent = loadertRes.Read<string>();
            loadertResContent = _macrosManager.ResolveMacros(loadertResContent, projectInfo);
            loadertRes.Write(loadertResContent);

            _resourceManager.CreateResource(projectInfo.RootDirectory, projectInfo.GitignoreFilename);

            FsOperation.CreateFileIfNotExists(projectInfo.ChangeLogPath);
            FsOperation.CreateFileIfNotExists(projectInfo.LicencePath);
            FsOperation.CreateFileIfNotExists(projectInfo.ReadmePath);
        }
    }
}
