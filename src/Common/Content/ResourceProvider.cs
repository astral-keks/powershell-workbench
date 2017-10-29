using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using System;

namespace AstralKeks.Workbench.Common.Content
{
    public interface IResourceProvider : IResourceReader, IResourceWriter
    {
    }

    public class FileResourceProvider : IResourceProvider
    {
        private readonly FileSystem _fileSystem;

        public FileResourceProvider(FileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public bool CanRead(string resourceName)
        {
            return !string.IsNullOrWhiteSpace(resourceName) && _fileSystem.FileExists(resourceName);
        }

        public string Read(string resourceName)
        {
            return _fileSystem.FileReadText(resourceName);
        }

        public bool CanWrite(string resourceName)
        {
            return !string.IsNullOrWhiteSpace(resourceName) && !_fileSystem.FileExists(resourceName);
        }

        public void Write(string resourceName, string resourceContent)
        {
            _fileSystem.FileCreate(resourceName, resourceContent);
        }
    }

    public class MemoryResourceProvider : IResourceProvider
    {
        private string _name;
        private string _content;

        public MemoryResourceProvider(string name, string content)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public bool CanRead(string resourceName)
        {
            return !string.IsNullOrWhiteSpace(resourceName) && resourceName == _name;
        }

        public bool CanWrite(string resourceName)
        {
            return !string.IsNullOrWhiteSpace(resourceName) && resourceName == _name;
        }

        public string Read(string resourceName)
        {
            return _content;
        }

        public void Write(string resourceName, string resourceContent)
        {
            _content = resourceContent;
        }
    }
}
