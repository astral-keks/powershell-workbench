using AstralKeks.Workbench.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AstralKeks.Workbench.Common.Resources
{
    public class ResourceManager
    {
        private readonly ResourceOrigin _origin;
        private readonly FileSystem _fileSystem;
        private readonly ResourceBundle _resourceBundle;
        private readonly IResourceFormat _format;

        public ResourceManager(ResourceOrigin origin, FileSystem fileSystem, ResourceBundle resourceBundle, IResourceFormat format = null)
        {
            _origin = origin ?? throw new ArgumentNullException(nameof(origin));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _resourceBundle = resourceBundle ?? throw new ArgumentNullException(nameof(resourceBundle));
            _format = format ?? new JsonResourceFormat();
        }

        public Resource CreateResource(IEnumerable<string> resourcePaths, string resourceName)
        {
            if (resourcePaths == null)
                throw new ArgumentNullException(nameof(resourcePaths));
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentException("Resource name is not set", nameof(resourceName));
            resourcePaths = resourcePaths.ToList();
            if (!resourcePaths.Any())
                throw new ArgumentException("Resource paths are not set", nameof(resourcePaths));

            var mainProvider = new FileResourceProvider(resourcePaths.First(), _fileSystem);
            var defaultProviders = resourcePaths.Skip(1)
                .Select(p => new FileResourceProvider(p, _fileSystem))
                .Cast<IResourceProvider>()
                .Union(new[] { new EmbeddedResourceProvider(resourceName, _origin, _resourceBundle) });

            CreateResource(mainProvider, new LinkedList<IResourceProvider>(defaultProviders).First);
            return new Resource(_format, mainProvider);
        }

        public Resource GetResource(IEnumerable<string> resourcePaths)
        {
            if (resourcePaths == null)
                throw new ArgumentNullException(nameof(resourcePaths));
            resourcePaths = resourcePaths.ToList();
            if (!resourcePaths.Any())
                throw new ArgumentException("Resource paths are not set", nameof(resourcePaths));

            var mainProvider = new FileResourceProvider(resourcePaths.First(), _fileSystem);
            var defaultProviders = resourcePaths.Skip(1)
                .Select(p => new FileResourceProvider(p, _fileSystem))
                .Cast<IResourceProvider>();

            var provider = FindResource(mainProvider, new LinkedList<IResourceProvider>(defaultProviders).First);
            return provider != null ? new Resource(_format, provider) : null;
        }

        private void CreateResource(IResourceProvider provider, LinkedListNode<IResourceProvider> defaultProviderNode)
        {
            var defaultProvider = defaultProviderNode?.Value;
            if (!provider.CanRead && defaultProvider != null)
            {
                CreateResource(defaultProvider, defaultProviderNode.Next);
                if (defaultProvider.CanRead)
                {
                    var resourceContent = defaultProvider.Read();
                    provider.Write(resourceContent);
                }
            }
        }

        private IResourceProvider FindResource(IResourceProvider provider, LinkedListNode<IResourceProvider> defaultProviderNode)
        {
            if (provider.CanRead)
                return provider;

            var defaultProvider = defaultProviderNode?.Value;
            if (defaultProvider != null)
                return FindResource(defaultProvider, defaultProviderNode.Next);
            else
                return null;
        }
    }
}
