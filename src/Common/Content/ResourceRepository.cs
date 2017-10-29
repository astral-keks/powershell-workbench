using AstralKeks.Workbench.Common.Infrastructure;

namespace AstralKeks.Workbench.Common.Content
{
    public class ResourceRepository
    {
        private readonly IResourceReader _defaultResourceReader;
        private readonly IResourceProvider _fileResourceProvider;

        public ResourceRepository(ResourceBundle resourceBundle, FileSystem fileSystem)
        {
            _defaultResourceReader = new EmbeddedResourceReader(resourceBundle);
            _fileResourceProvider = new FileResourceProvider(fileSystem);
        }

        public IResource CreateResource(string resourcePath, string resourceName)
        {
            if (_fileResourceProvider.CanWrite(resourcePath) && _defaultResourceReader.CanRead(resourceName))
            {
                var resourceContent = _defaultResourceReader.Read(resourceName);
                _fileResourceProvider.Write(resourcePath, resourceContent);
            }

            return GetResource(resourcePath);
        }

        public IResource CreateResource(string resourcePath)
        {
            if (_fileResourceProvider.CanWrite(resourcePath))
                _fileResourceProvider.Write(resourcePath, string.Empty);

            return GetResource(resourcePath);
        }

        public IResource GetResource(string resourcePath)
        {
            IResource resource = null;

            if (_fileResourceProvider.CanRead(resourcePath))
                resource = new Resource(resourcePath, _fileResourceProvider, _fileResourceProvider);

            return resource;
        }
    }
}
