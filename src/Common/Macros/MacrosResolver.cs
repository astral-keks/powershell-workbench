using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.Workbench.Macros
{
    public class MacrosResolver
    {
        private readonly FileSystem _fileSystem;
        private readonly SystemVariable _systemVariable;

        public MacrosResolver(FileSystem fileSystem, SystemVariable systemVariable)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
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
                .Replace("{$Bin}", PreparePath(_fileSystem.BinDirectory()))
                .Replace("{$User}", PreparePath(_systemVariable.UserDirectory))
                .Replace("{$Workspace}", PreparePath(_systemVariable.WorkspaceDirectory))
                .Replace("{$Userspace}", PreparePath(_systemVariable.UserspaceDirectory));
        }

        private string PreparePath(string path)
        {
            return path.TrimEnd(Path.DirectorySeparatorChar);
        }
    }
}
