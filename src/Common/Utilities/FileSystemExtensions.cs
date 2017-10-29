using AstralKeks.Workbench.Common.Infrastructure;
using System;
using System.IO;
using System.Reflection;

namespace AstralKeks.Workbench.Common.Utilities
{
    public static class FileSystemExtensions
    {
        //public static string GetAbsolutePath(this FileSystem fileSystem, string parentPath, string middlePath, string childPath)
        //{
        //    parentPath = fileSystem.GetAbsolutePath(parentPath, middlePath);
        //    return fileSystem.GetAbsolutePath(parentPath, childPath);
        //}

        //public static string GetAbsolutePath(this FileSystem fileSystem, string parentPath, string childPath = null)
        //{
        //    var combinedPath = Path.Combine(parentPath, childPath ?? string.Empty);
        //    if (!Path.IsPathRooted(combinedPath))
        //        combinedPath = Path.Combine(fileSystem.GetCurrentDirectory(), combinedPath);
        //    return fileSystem.GetFullPath(new Uri(combinedPath).LocalPath);
        //}

        public static string MakeAbsolute(this FileSystem fileSystem, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = ".";
            return fileSystem.GetFullPath(path);
        }
        
        public static void FileCreate(this FileSystem fileSystem, string path, string content = null)
        {
            path = fileSystem.MakeAbsolute(path);

            fileSystem.DirectoryCreate(Path.GetDirectoryName(path));
            fileSystem.FileWriteText(path, content ?? string.Empty);
        }

        public static void FileCreate(this FileSystem fileSystem, string path, string[] content)
        {
            path = fileSystem.MakeAbsolute(path);

            fileSystem.DirectoryCreate(Path.GetDirectoryName(path));
            fileSystem.FileWriteLines(path, content ?? new string[0]);
        }

        public static string FindParentDirectory(this FileSystem fileSystem, string innerDirectory, Func<string, bool> predicate)
        {
            innerDirectory = fileSystem.MakeAbsolute(innerDirectory);

            var workspaceDirectory = innerDirectory;
            while (workspaceDirectory != null && !predicate(workspaceDirectory))
                workspaceDirectory = Path.GetDirectoryName(workspaceDirectory);

            return workspaceDirectory != null ? fileSystem.GetFullPath(workspaceDirectory) : null;
        }
        
        public static string BinDirectory(this FileSystem fileSystem)
        {
            var codeBase = typeof(FileSystem).GetTypeInfo().Assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(fileSystem.MakeAbsolute(path));
        }
    }
}
