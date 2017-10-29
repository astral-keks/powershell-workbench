using System;
using System.Linq;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Resources;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Template;

namespace AstralKeks.Workbench.Controllers
{
    public class ToolkitController
    {
        private readonly FileSystem _fileSystem;
        private readonly TemplateProcessor _templateProcessor;
        private readonly ResourceRepository _resourceRepository;

        public ToolkitController(FileSystem fileSystem, TemplateProcessor templateProcessor, ResourceRepository resourceRepository)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
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
                CoreProjectFileName = $"{toolkitName}.{Directories.Core}.csproj",
                CoreProjectPath = PathBuilder.Combine(coreDirectory, $"{toolkitName}.{Directories.Core}.csproj"),
                CommandProjectFileName = $"{toolkitName}.{Directories.Command}.csproj",
                CommandProjectPath = PathBuilder.Combine(commandDirectory, $"{toolkitName}.{Directories.Command}.csproj"),
                CmdletDirectory = PathBuilder.Combine(commandDirectory, Directories.Cmdlet),
                ManifestPath = PathBuilder.Combine(commandDirectory, Files.ManifestPsd1),
                LoaderPath = PathBuilder.Combine(commandDirectory, Files.LoaderPsm1),
                GitignorePath = PathBuilder.Combine(directory, Files.Gitignore),
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

            var model = TemplateModel.Object(project);

            var slnRes = _resourceRepository.CreateResource(project.SolutionPath, Files.ProjectSln);
            slnRes.Content = _templateProcessor.Transform(slnRes.Content, model);

            var coreRes = _resourceRepository.CreateResource(project.CoreProjectPath, Files.CoreCsproj);
            coreRes.Content = _templateProcessor.Transform(coreRes.Content, model);

            var commandRes = _resourceRepository.CreateResource(project.CommandProjectPath, Files.CommandCsproj);
            commandRes.Content = _templateProcessor.Transform(commandRes.Content, model);

            var manifestRes = _resourceRepository.CreateResource(project.ManifestPath, Files.ManifestPsd1);
            manifestRes.Content = _templateProcessor.Transform(manifestRes.Content, model);

            var loadertRes = _resourceRepository.CreateResource(project.LoaderPath, Files.LoaderPsm1);
            loadertRes.Content = _templateProcessor.Transform(loadertRes.Content, model);

            _resourceRepository.CreateResource(project.GitignorePath, Files.Gitignore);

            _fileSystem.FileCreate(project.ChangeLogPath);
            _fileSystem.FileCreate(project.LicencePath);
            _fileSystem.FileCreate(project.ReadmePath);
        }
    }
}
