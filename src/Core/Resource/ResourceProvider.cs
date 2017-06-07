using System;
using System.IO;
using System.Reflection;

namespace AstralKeks.Workbench.Core.Resource
{
    public interface IResourceProvider
    {
        string Read();
        void Write(string resource);
    }

    public class FileResourceProvider : IResourceProvider
    {
        private readonly string _filePath;

        public FileResourceProvider(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            _filePath = filePath;
            var directory = Path.GetDirectoryName(_filePath);
            if (directory != null && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public string Read()
        {
            return File.Exists(_filePath) ? File.ReadAllText(_filePath) : null;
        }

        public void Write(string resource)
        {
            File.WriteAllText(_filePath, resource);
        }
    }

    public class EmbeddedResourceProvider : IResourceProvider
    {
        private readonly Assembly _assembly;
        private readonly string _resourceName;

        public EmbeddedResourceProvider(string resourceName, Assembly assembly = null)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentException("Resource name is not set", nameof(resourceName));

            _resourceName = resourceName;
            _assembly = assembly ?? typeof(EmbeddedResourceProvider).GetTypeInfo().Assembly;
        }

        public string Read()
        {
            using (var stream = _assembly.GetManifestResourceStream(_resourceName))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
                else
                {
                    throw new InvalidOperationException($"Cannot find resource {_resourceName}");
                }
            }
        }

        public void Write(string resource)
        {
            throw new NotSupportedException();
        }
    }
}
