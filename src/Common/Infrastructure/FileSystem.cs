using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public void FileDelete(string path)
        {
            File.Delete(path);
        }

        public void FileWriteText(string path, string text)
        {
            File.WriteAllText(path, text);
        }

        public void FileWriteLines(string path, string[] lines, bool append = false)
        {
            if (append)
                File.AppendAllLines(path, lines);
            else
                File.WriteAllLines(path, lines);
        }

        public string FileReadText(string path)
        {
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        public IEnumerable<string> FileReadLines(string path)
        {
            return File.Exists(path) ? File.ReadLines(path) : Enumerable.Empty<string>();
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void DirectoryCreate(string path)
        {
            Directory.CreateDirectory(path);
        }

        public string[] DirectoryList(string path)
        {
            return Directory.GetFileSystemEntries(path);
        }

        public string DirectoryGetCurrent()
        {
            return Directory.GetCurrentDirectory();
        }

        public void DirectorySetCurrent(string directory)
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
