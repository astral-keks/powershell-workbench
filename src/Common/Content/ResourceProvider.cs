using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Template;
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
            _fileSystem.FileWriteText(resourceName, resourceContent);
        }
    }

    public class TemplateResourceProvider : IResourceProvider
    {
        private readonly IResourceProvider _innerProvider;
        private readonly TemplateProcessor _templateProcessor;

        public TemplateResourceProvider(IResourceProvider innerProvider, TemplateProcessor templateProcessor)
        {
            _innerProvider = innerProvider ?? throw new ArgumentNullException(nameof(innerProvider));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        }

        public bool CanRead(string resourceName)
        {
            return _innerProvider.CanRead(resourceName);
        }

        public bool CanWrite(string resourceName)
        {
            return false;
        }

        public string Read(string resourceName)
        {
            var resourceContent = _innerProvider.Read(resourceName);
            return _templateProcessor.Transform(resourceContent);
        }

        public void Write(string resourceName, string resourceContent)
        {
            throw new NotSupportedException();
        }
    }
}
