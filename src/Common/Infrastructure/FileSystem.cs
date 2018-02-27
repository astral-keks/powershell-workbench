using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.Workbench.Common.Infrastructure
{
    public class FileSystem
    {
        public const char WindowsSeparator = '\\';
        public const char UnixSeparator = '/';

        public virtual char Separator
        {
            get { return Path.DirectorySeparatorChar; }
        }

        public virtual string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public virtual bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public virtual void FileCopy(string sourcePath, string destinationPath, bool overwrite = false)
        {
            File.Copy(sourcePath, destinationPath, overwrite);
        }

        public virtual void FileDelete(string path)
        {
            File.Delete(path);
        }

        public virtual void FileWriteBytes(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        public virtual void FileWriteText(string path, string text)
        {
            File.WriteAllText(path, text);
        }

        public virtual void FileWriteLines(string path, string[] lines, bool append = false)
        {
            if (append)
                File.AppendAllLines(path, lines);
            else
                File.WriteAllLines(path, lines);
        }

        public virtual byte[] FileReadBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public virtual string FileReadText(string path)
        {
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        public virtual IEnumerable<string> FileReadLines(string path)
        {
            return File.Exists(path) ? File.ReadLines(path) : Enumerable.Empty<string>();
        }

        public virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public virtual void DirectoryCreate(string path)
        {
            Directory.CreateDirectory(path);
        }

        public virtual void DirectoryDelete(string path, bool recursive = false)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, recursive);
        }

        public virtual string[] DirectoryList(string path)
        {
            return Directory.Exists(path) ? Directory.GetFileSystemEntries(path) : new string[0];
        }

        public virtual string DirectoryGetCurrent()
        {
            return Directory.GetCurrentDirectory();
        }

        public virtual void DirectorySetCurrent(string directory)
        {
            Directory.SetCurrentDirectory(directory);
        }

        public virtual string[] GetDirectories(string path)
        {
            return Directory.Exists(path) ? Directory.GetDirectories(path) : new string[0];
        }

        public virtual string[] GetFiles(string path)
        {
            return Directory.Exists(path) ? Directory.GetFiles(path) : new string[0];
        }
    }
}
