using System;
using System.IO;

namespace AstralKeks.Workbench.Core.Management
{
    public class FileSystemManager
    {
        public string GetUserDirectoryPath()
        {
            switch (OperatingSystemManager.Current)
            {
                case OperatingSystemManager.Windows:
                    return Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"));
                case OperatingSystemManager.Linux:
                    return Path.Combine(Environment.GetEnvironmentVariable("HOME"));
                default:
                    throw new NotSupportedException();
            }
        }

        public string GetAbsolutePath(string parentPath, string middlePath, string childPath)
        {
            parentPath = GetAbsolutePath(parentPath, middlePath);
            return GetAbsolutePath(parentPath, childPath);
        }

        public string GetAbsolutePath(string parentPath, string childPath = null)
        {
            var combinedPath = Path.Combine(parentPath, childPath ?? string.Empty);
            if (!Path.IsPathRooted(combinedPath))
                combinedPath = Path.Combine(Directory.GetCurrentDirectory(), combinedPath);
            return Path.GetFullPath(new Uri(combinedPath).LocalPath);
        }

        public string GetBinDirectoryPath()
        {
            //var codeBase = typeof(FileSystemManager).GetTypeInfo().Assembly.CodeBase;
            //var uri = new UriBuilder(codeBase);
            //var path = Uri.UnescapeDataString(uri.Path);
            //return Path.GetDirectoryName(path);
            return AppContext.BaseDirectory;
        }

        public string FindParentDirectory(string innerDirectory, Func<string, bool> predicate)
        {
            innerDirectory = GetAbsolutePath(innerDirectory);

            var workspaceDirectory = innerDirectory;
            while (workspaceDirectory != null && !predicate(workspaceDirectory))
                workspaceDirectory = Path.GetDirectoryName(workspaceDirectory);

            return workspaceDirectory ?? innerDirectory;
        }

        public void CreateDirectoryIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public void CreateFileIfNotExists(string path, string content)
        {
            CreateDirectoryIfNotExists(Path.GetDirectoryName(path));
            if (!File.Exists(path))
                File.WriteAllText(path, content);
        }
    }
}
