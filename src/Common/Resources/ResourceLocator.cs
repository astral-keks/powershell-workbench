using AstralKeks.Workbench.Common.FileSystem;
using System;

namespace AstralKeks.Workbench.Common.Resources
{
    public class ResourceLocator
    {
        private readonly string _directory;
        private readonly string _filename;

        public ResourceLocator(string location, string filename)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename));

            _directory = FsPath.Absolute(location);
            _filename = filename;
        }

        public ResourceLocator(string location, string directory, string filename)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename));

            _directory = FsPath.Absolute(location, directory);
            _filename = filename;
        }

        public string Directory => _directory;

        public string Filename => _filename;

        public string Path => FsPath.Absolute(_directory, _filename);
    }
}
