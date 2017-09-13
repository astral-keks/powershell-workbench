using System;
using System.Linq;
using AstralKeks.Workbench.Common.Resources;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Resources;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Macros;
using AstralKeks.Workbench.Common.Infrastructure;

namespace AstralKeks.Workbench.Controllers
{
    public class ToolkitController
    {
        private readonly FileSystem _fileSystem;
        private readonly MacrosResolver _macrosManager;
        private readonly ResourceManager _resourceManager;

        public ToolkitController(FileSystem fileSystem, MacrosResolver macrosManager, ResourceManager resourceManager)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _macrosManager = macrosManager ?? throw new ArgumentNullException(nameof(macrosManager));
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        }

        public void InitializeToolkitProject(string directory, string toolkitName, string toolkitAuthor)
        {
            if (string.IsNullOrWhiteSpace(toolkitName))
                throw new ArgumentException("Toolkit name is not set", nameof(toolkitName));
            if (string.IsNullOrWhiteSpace(toolkitAuthor))
                throw new ArgumentException("Toolkit author is not set", nameof(toolkitAuthor));

            directory = _fileSystem.GetFullPath(directory);
            var srcDirectory = PathBuilder.Combine(directory, Directories.Source);
            var coreDirectory = PathBuilder.Combine(srcDirectory, Directories.Core);
            var commandDirectory = PathBuilder.Combine(srcDirectory, Directories.Command);

            var author = new string(toolkitAuthor.Where(char.IsLetterOrDigit).ToArray());
            var moduleName = $"{author}.{toolkitName}";
            var moduleVersion = "0.0.1";
            var targetFramework = "net462";

            var project = new Project
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
                SolutionPath = PathBuilder.Combine(srcDirectory, $"{toolkitName}.sln"),
                CoreProjectPath = PathBuilder.Combine(coreDirectory, $"{toolkitName}.{Directories.Core}.csproj"),
                CommandProjectPath = PathBuilder.Combine(commandDirectory, $"{toolkitName}.{Directories.Command}.csproj"),
                LoaderSourceFilename = Files.LoaderPsm1,
                ManifestSourceFilename = Files.ManifestPsd1,
                GitignoreFilename = Files.Gitignore,
                CmdletDirectory = PathBuilder.Combine(commandDirectory, Directories.Cmdlet),
                ChangeLogPath = PathBuilder.Combine(directory, Files.ChangeLogMd),
                LicencePath = PathBuilder.Combine(directory, Files.LicenceMd),
                ReadmePath = PathBuilder.Combine(directory, Files.ReadmeMd),
            };

            InitializeToolkitProject(project);
        }

        public void InitializeToolkitProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            var slnRes = _resourceManager.CreateResource(new[] { project.SolutionPath }, Files.ProjectSln);
            var slnResContent = slnRes.Read<string>();
            slnResContent = _macrosManager.ResolveMacros(slnResContent, project);
            slnRes.Write(slnResContent);

            var coreRes = _resourceManager.CreateResource(new[] { project.CoreProjectPath }, Files.CoreCsproj);
            var coreResContent = coreRes.Read<string>();
            coreResContent = _macrosManager.ResolveMacros(coreResContent, project);
            coreRes.Write(coreResContent);

            var commandRes = _resourceManager.CreateResource(new[] { project.CommandProjectPath }, Files.CommandCsproj);
            var commandResContent = commandRes.Read<string>();
            commandResContent = _macrosManager.ResolveMacros(commandResContent, project);
            commandRes.Write(commandResContent);

            var manifestRes = _resourceManager.CreateResource(new[] { project.CommandProjectPath }, project.ManifestSourceFilename);
            var manifestResContent = manifestRes.Read<string>();
            manifestResContent = _macrosManager.ResolveMacros(manifestResContent, project);
            manifestRes.Write(manifestResContent);

            var loadertRes = _resourceManager.CreateResource(new[] { project.CommandProjectPath }, project.LoaderSourceFilename);
            var loadertResContent = loadertRes.Read<string>();
            loadertResContent = _macrosManager.ResolveMacros(loadertResContent, project);
            loadertRes.Write(loadertResContent);

            _resourceManager.CreateResource(new[] { project.RootDirectory }, project.GitignoreFilename);

            _fileSystem.FileCreate(project.ChangeLogPath);
            _fileSystem.FileCreate(project.LicencePath);
            _fileSystem.FileCreate(project.ReadmePath);
        }
    }
}
