using AstralKeks.Workbench.Common.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.Workbench.Core.Management
{
    public class MacrosManager
    {
        private readonly WorkspaceManager _workspaceManager;
        private readonly UserspaceManager _userspaceManager;

        public MacrosManager(WorkspaceManager workspaceManager, UserspaceManager userspaceManager)
        {
            _workspaceManager = workspaceManager ?? throw new ArgumentNullException(nameof(workspaceManager));
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
        }

        public string ResolveMacros(string input, object data)
        {
            var type = data.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(data)?.ToString();
                input = input.Replace($"{{${propertyName}}}", propertyValue);
            }

            return input;
        }
        
        public string ResolveMacros(string input, string pipeline, List<string> arguments)
        {
            return ResolveMacros(input, arguments)
                .Replace("{$Pipeline}", pipeline ?? string.Empty);
        }

        public string ResolveMacros(string input, List<string> arguments)
        {
            return ResolveMacros(input)
                .Replace("{$Args}", string.Join(" ", arguments ?? Enumerable.Empty<string>()));
        }

        public string ResolveMacros(string input)
        {
            return (input ?? string.Empty)
                .Replace("{$Bin}", PreparePath(FsPath.BinDirectory()))
                .Replace("{$User}", PreparePath(FsPath.UserDirectory()))
                .Replace("{$Workspace}", PreparePath(_workspaceManager.GetWorkspaceDirectory()))
                .Replace("{$Userspace}", PreparePath(_userspaceManager.GetUserspaceDirectory()));
        }

        private string PreparePath(string path)
        {
            return path.TrimEnd(Path.DirectorySeparatorChar);
        }
    }
}
