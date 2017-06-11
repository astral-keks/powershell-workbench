using AstralKeks.Workbench.Core.Resource;
using System;

namespace AstralKeks.Workbench.Core.Management
{
    public class ResourceManager
    {
        private readonly FileSystemManager _fileSystemManager;

        public ResourceManager(FileSystemManager fileSystemManager)
        {
            _fileSystemManager = fileSystemManager ?? throw new ArgumentNullException(nameof(fileSystemManager));
        }

        public void CreateResource(string workspaceDirectory, string userspaceDirectory, 
            string directory, string filename)
        {
            var workspacePath = GetResourcePath(workspaceDirectory, directory, filename);
            var userspacePath = GetResourcePath(userspaceDirectory, directory, filename);
            var embeddedResourceName = GetResourceName(filename);

            var workspaceProvider = new FileResourceProvider(workspacePath);
            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName);
            var format = new StringResourceFormat();

            var resource = new Resource<string>(format, workspaceProvider, userspaceProvider, defaults);
            resource.Read();
        }

        public void CreateResource(string userspaceDirectory, string directory, string filename)
        {
            var userspacePath = GetResourcePath(userspaceDirectory, directory, filename);
            var embeddedResourceName = GetResourceName(filename);

            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName);
            var format = new StringResourceFormat();

            var resource = new Resource<string>(format, userspaceProvider, defaults);
            resource.Read();
        }

        public Resource<TObject> GetResource<TObject>(string workspaceDirectory, string userspaceDirectory, 
            string directory, string filename)
        {
            var workspacePath = GetResourcePath(workspaceDirectory, directory, filename);
            var userspacePath = GetResourcePath(userspaceDirectory, directory, filename);
            var embeddedResourceName = GetResourceName(filename);

            var workspaceProvider = new FileResourceProvider(workspacePath);
            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName);
            var format = new JsonResourceFormat<TObject>();

            return new Resource<TObject>(format, workspaceProvider, userspaceProvider, defaults);
        }

        public Resource<TObject> GetResource<TObject>(string userspaceDirectory, string directory, string filename)
        {
            var userspacePath = GetResourcePath(userspaceDirectory, directory, filename);
            var embeddedResourceName = GetResourceName(filename);

            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName);
            var format = new JsonResourceFormat<TObject>();

            return new Resource<TObject>(format, userspaceProvider, defaults);
        }

        public string GetResourcePath(string rootDirectory, string directory, string filename)
        {
            return _fileSystemManager.GetAbsolutePath(rootDirectory, directory, filename);
        }

        public string[] GetResourcePaths(string rootDirectory, string directory)
        {
            return _fileSystemManager.GetFilesInDirectory(rootDirectory, directory);
        }

        public void DeleteResource(string rootDirectory, string directory, string filename)
        {
            var resourcePath = GetResourcePath(rootDirectory, directory, filename);
            _fileSystemManager.DeleeteFile(resourcePath);
        }

        public string GetResourceName(string name)
        {
            return $"AstralKeks.Workbench.Core.Resource.{OperatingSystemManager.Current}.{name}";
        }
    }
}
