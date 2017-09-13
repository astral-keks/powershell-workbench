﻿
namespace AstralKeks.Workbench.Models
{
    public class Project
    {
        public string GeneratedOn { get; set; }

        public string ModuleName { get; set; }
        public string ModuleVersion { get; set; }
        public string ModuleAuthor { get; set; }
        public string ModuleGuid { get; set; }
        public string ModuleCopyright { get; set; }

        public string TargetFramework { get; set; }
        public string CoreProjectName { get; set; }
        public string CoreAssemblyName { get; set; }
        public string CommandProjectName { get; set; }
        public string CommandAssemblyName { get; set; }
        public string OutputDirectoryPart { get; set; }

        public string RootDirectory { get; set; }
        public string SolutionPath { get; set; }
        public string SolutionFilename { get; set; }
        public string CoreProjectPath { get; set; }
        public string CommandProjectPath { get; set; }
        public string LoaderSourceFilename { get; set; }
        public string ManifestSourceFilename { get; set; }
        public string GitignoreFilename { get; set; }
        public string CmdletDirectory { get; set; }
        public string ChangeLogPath { get; set; }
        public string LicencePath { get; set; }
        public string ReadmePath { get; set; }
    }
}