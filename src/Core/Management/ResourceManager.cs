using AstralKeks.Workbench.Core.Resources;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AstralKeks.Workbench.Core.Management
{
    public class ResourceManager
    {
        private readonly FileSystemManager _fileSystemManager;

        public ResourceManager(FileSystemManager fileSystemManager)
        {
            _fileSystemManager = fileSystemManager ?? throw new ArgumentNullException(nameof(fileSystemManager));
        }

        public Resource GetResource(string workspaceDirectory, string userspaceDirectory, 
            string directory, string filename)
        {
            var workspacePath = GetResourcePath(workspaceDirectory, directory, filename);
            var userspacePath = GetResourcePath(userspaceDirectory, directory, filename);
            var embeddedResourceName = GetResourceName(filename);

            var workspaceProvider = new FileResourceProvider(workspacePath);
            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName);
            var format = new JsonResourceFormat();

            return new Resource(format, workspaceProvider, userspaceProvider, defaults);
        }

        public Resource GetResource(string userspaceDirectory, string directory, string filename)
        {
            var userspacePath = GetResourcePath(userspaceDirectory, directory, filename);
            var embeddedResourceName = GetResourceName(filename);

            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName);
            var format = new JsonResourceFormat();

            return new Resource(format, userspaceProvider, defaults);
        }

        public Resource GetResource(string targetDirectory, string filename)
        {
            var userspacePath = GetResourcePath(targetDirectory, filename);
            var embeddedResourceName = GetResourceName(filename);

            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName);
            var format = new JsonResourceFormat();

            return new Resource(format, userspaceProvider, defaults);
        }

        public void CreateResource(string workspaceDirectory, string userspaceDirectory, string directory, string filename)
        {
            GetResource(workspaceDirectory, userspaceDirectory, directory, filename);
        }

        public void CreateResource(string userspaceDirectory, string directory, string filename)
        {
            GetResource(userspaceDirectory, directory, filename);
        }

        public void CreateResource(string targetDirectory, string filename)
        {
            GetResource(targetDirectory, filename);
        }

        public void DeleteResource(string rootDirectory, string directory, string filename)
        {
            var resourcePath = GetResourcePath(rootDirectory, directory, filename);
            _fileSystemManager.DeleeteFile(resourcePath);
        }

        public string GetResourcePath(string rootDirectory, string directory, string filename)
        {
            return _fileSystemManager.GetAbsolutePath(rootDirectory, directory, filename);
        }

        public string GetResourcePath(string rootDirectory, string filename)
        {
            return _fileSystemManager.GetAbsolutePath(rootDirectory, filename);
        }

        public string[] GetResourcePaths(string rootDirectory, string directory)
        {
            return _fileSystemManager.GetFilesInDirectory(rootDirectory, directory);
        }

        public string GetResourceName(string filename)
        {
            var resourceQuery = $"AstralKeks.Workbench.Core.Resources.{OperatingSystemManager.CurrentOS}.{filename}";

            var resourceNames = typeof(EmbeddedResourceProvider).GetTypeInfo().Assembly.GetManifestResourceNames();
            var resourceName = resourceNames
                .Where(r => r.Contains(OperatingSystemManager.CurrentOS))
                .FirstOrDefault(r => r == resourceQuery);
            if (resourceName == null)
            {
                resourceName = resourceNames
                    .Where(r => r.Contains(OperatingSystemManager.CurrentOS))
                    .FirstOrDefault(r => Regex.IsMatch(resourceQuery, r));
            }            
            if (resourceName == null)
                throw new ArgumentException($"Embedded resource {filename} was not found");

            return resourceName;
        }
    }
}
