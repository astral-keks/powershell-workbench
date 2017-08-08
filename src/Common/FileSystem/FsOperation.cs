using System;
using System.IO;

namespace AstralKeks.Workbench.Common.FileSystem
{
    public class FsOperation
    {
        public static string[] GetFilesInDirectory(string parentPath, string childPath = null)
        {
            var path = FsPath.Absolute(parentPath, childPath);
            return Directory.GetFiles(path);
        }

        public static string FindParentDirectory(string innerDirectory, Func<string, bool> predicate)
        {
            innerDirectory = FsPath.Absolute(innerDirectory);

            var workspaceDirectory = innerDirectory;
            while (workspaceDirectory != null && !predicate(workspaceDirectory))
                workspaceDirectory = Path.GetDirectoryName(workspaceDirectory);

            return workspaceDirectory;
        }

        public static void CreateDirectoryIfNotExists(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public static void CreateFileIfNotExists(string path, string content = null)
        {
            CreateDirectoryIfNotExists(Path.GetDirectoryName(path));
            if (!File.Exists(path))
                File.WriteAllText(path, content ?? string.Empty);
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public static string ReadFile(string path)
        {
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }
    }
}
