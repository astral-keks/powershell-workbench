using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using System;
using System.IO;

namespace AstralKeks.Workbench.Common.Resources
{
    public interface IResourceProvider
    {
        bool CanRead { get; }
        string Read();
        void Write(string resource);
    }

    public class FileResourceProvider : IResourceProvider
    {
        private readonly string _filePath;
        private readonly FileSystem _fileSystem;

        public FileResourceProvider(string filePath, FileSystem fileSystem)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));
            if (fileSystem == null)
                throw new ArgumentNullException(nameof(fileSystem));

            _filePath = filePath;
            _fileSystem = fileSystem;
            var directory = Path.GetDirectoryName(_filePath);
            if (directory != null)
                _fileSystem.DirectoryCreate(directory);
        }

        public bool CanRead => File.Exists(_filePath);

        public string Read()
        {
            return _fileSystem.FileRead(_filePath);
        }

        public void Write(string resource)
        {
            _fileSystem.FileWriteAllText(_filePath, resource);
        }
    }

    public class EmbeddedResourceProvider : IResourceProvider
    {
        private readonly ResourceBundle _resourceBundle;
        private readonly string _resourceName;
        private readonly Type _assemblyLocator;

        public EmbeddedResourceProvider(string resourceName, ResourceOrigin origin, ResourceBundle resourceBundle)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentException("Resource name is not set", nameof(resourceName));
            if (origin == null)
                throw new ArgumentNullException(nameof(origin));
            if (resourceBundle == null)
                throw new ArgumentNullException(nameof(resourceBundle));

            _resourceName = resourceName;
            _assemblyLocator = origin.AssemblyLocator;
            _resourceBundle = resourceBundle;
        }

        public bool CanRead => _resourceBundle.ExistsResource(_resourceName, _assemblyLocator);

        public string Read()
        {
            return _resourceBundle.GetResource(_resourceName, _assemblyLocator);
        }

        public void Write(string resource)
        {
            throw new NotSupportedException();
        }
    }
}
