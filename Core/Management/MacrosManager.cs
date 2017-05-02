using System;
using System.Collections.Generic;
using System.IO;

namespace AstralKeks.Workbench.Core.Management
{
    public class MacrosManager
    {
        private readonly FileSystemManager _fileSystemManager;

        public MacrosManager(FileSystemManager fileSystemManager)
        {
            _fileSystemManager = fileSystemManager ?? throw new ArgumentNullException(nameof(fileSystemManager));
        }

        public string ResolveMacros(string input, List<string> arguments)
        {
            return ResolveMacros(input)
                .Replace("{$Args}", string.Join(" ", arguments));
        }

        public string ResolveMacros(string input)
        {
            return input
                .Replace("{$Bin}", PreparePath(_fileSystemManager.GetBinDirectoryPath()))
                .Replace("{$User}", PreparePath(_fileSystemManager.GetUserDirectoryPath()));
        }

        private string PreparePath(string path)
        {
            return path.TrimEnd(Path.DirectorySeparatorChar);
        }

    }
}
