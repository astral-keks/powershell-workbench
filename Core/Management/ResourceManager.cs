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

        public void InitializeResource(string workspaceDirectory, string userspaceDirectory, 
            string directory, string filename)
        {
            var workspacePath = _fileSystemManager.GetAbsolutePath(workspaceDirectory, directory, filename);
            var userspacePath = _fileSystemManager.GetAbsolutePath(userspaceDirectory, directory, filename);
            var embeddedResourceName = GetResourceName(filename);

            var workspaceProvider = new FileResourceProvider(workspacePath);
            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName);
            var format = new StringResourceFormat();

            var resource = new Resource<string>(format, workspaceProvider, userspaceProvider, defaults);
            resource.Read();
        }


        public Resource<TObject> GetResource<TObject>(string workspaceDirectory, string userspaceDirectory, 
            string directory, string filename)
        {
            var workspacePath = _fileSystemManager.GetAbsolutePath(workspaceDirectory, directory, filename);
            var userspacePath = _fileSystemManager.GetAbsolutePath(userspaceDirectory, directory, filename);
            var embeddedResourceName = GetResourceName(filename);

            var workspaceProvider = new FileResourceProvider(workspacePath);
            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName);
            var format = new JsonResourceFormat<TObject>();

            return new Resource<TObject>(format, workspaceProvider, userspaceProvider, defaults);
        }

        public Resource<TObject> GetResource<TObject>(string userspaceDirectory, string directory, string filename)
        {
            var userspacePath = _fileSystemManager.GetAbsolutePath(userspaceDirectory, directory, filename);
            var embeddedResourceName = GetResourceName(filename);

            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName);
            var format = new JsonResourceFormat<TObject>();

            return new Resource<TObject>(format, userspaceProvider, defaults);
        }

        public string GetResourceName(string name)
        {
            return $"{Data.Resource.Namespace}.{OperatingSystemManager.Current}.{name}";
        }
    }
}
