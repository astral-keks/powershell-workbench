using AstralKeks.Workbench.Core.Data;
using System;
using System.IO;

namespace AstralKeks.Workbench.Core.Management
{
    public class UserspaceManager
    {
        private readonly FileSystemManager _fileSystemManager;

        public UserspaceManager(FileSystemManager fileSystemManager)
        {
            _fileSystemManager = fileSystemManager ?? throw new ArgumentNullException(nameof(fileSystemManager));
        }

        public string GetUserspaceDirectory()
        {
            return Path.Combine(_fileSystemManager.GetUserDirectoryPath(), FileSystem.WorkbenchDirectory);
        }
    }
}
