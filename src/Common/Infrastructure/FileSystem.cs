using System;
using System.IO;

namespace AstralKeks.Workbench.Common.Infrastructure
{
    public class FileSystem
    {
        public const char WindowsSeparator = '\\';
        public const char UnixSeparator = '/';

        public char Separator
        {
            get { return Path.DirectorySeparatorChar; }
        }

        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void FileWriteAllText(string path, string text)
        {
            File.WriteAllText(path, text);
        }

        public void FileDelete(string path)
        {
            File.Delete(path);
        }

        public string FileRead(string path)
        {
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void DirectoryCreate(string path)
        {
            Directory.CreateDirectory(path);
        }

        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public void SetCurrentDirectory(string directory)
        {
            Directory.SetCurrentDirectory(directory);
        }

        public string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }
    }
}
