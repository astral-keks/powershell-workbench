using System.IO.Abstractions.TestingHelpers;

namespace AstralKeks.Workbench.Infrastructure
{
    internal class FileSystemMockup : FileSystem
    {
        private readonly MockFile _file;
        private readonly MockPath _path;
        private readonly MockDirectory _directory;

        public FileSystemMockup(IMockFileDataAccessor data)
        {
            _file = new MockFile(data);
            _path = new MockPath(data);
            _directory = new MockDirectory(data, _file, data.Directory.GetCurrentDirectory());
        }

        public override char Separator
        {
            get { return _path.DirectorySeparatorChar; }
        }

        public override string GetFullPath(string path)
        {
            return _path.GetFullPath(path);
        }

        public override bool FileExists(string path)
        {
            return _file.Exists(path);
        }

        public override void FileCopy(string source_path, string destination_path, bool overwrite = false)
        {
            _file.Copy(source_path, destination_path, overwrite);
        }

        public override void FileWriteText(string path, string text)
        {
            _file.WriteAllText(path, text);
        }

        public override string FileReadText(string path)
        {
            return _file.Exists(path) ? _file.ReadAllText(path) : null;
        }

        public override bool DirectoryExists(string path)
        {
            return _directory.Exists(path);
        }

        public override void DirectoryCreate(string path)
        {
            _directory.CreateDirectory(path);
        }

        public override void DirectoryDelete(string path, bool recursive = false)
        {
            if (_directory.Exists(path))
                _directory.Delete(path, recursive);
        }

        public override string[] DirectoryList(string path)
        {
            return _directory.Exists(path) ? _directory.GetFileSystemEntries(path) : new string[0];
        }

        public override string DirectoryGetCurrent()
        {
            return _directory.GetCurrentDirectory();
        }

        public override void DirectorySetCurrent(string directory)
        {
            _directory.SetCurrentDirectory(directory);
        }

        public override string[] GetDirectories(string path)
        {
            return _directory.Exists(path) ? _directory.GetDirectories(path) : new string[0];
        }
    }
}
