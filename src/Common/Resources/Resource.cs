using System;
using System.Collections.Generic;
using System.Linq;

namespace AstralKeks.Workbench.Common.Resources
{
    public class Resource
    {
        private readonly IResourceFormat _format;
        private readonly IResourceProvider _provider;
        private readonly LinkedList<IResourceProvider> _defaultsProviders;

        public Resource(IResourceFormat format, IResourceProvider provider, params IResourceProvider[] defaultsProviders)
        {
            if (!defaultsProviders.Any())
                throw new ArgumentException("No configuration providers have been specified", nameof(defaultsProviders));

            _format = format ?? throw new ArgumentNullException(nameof(format));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _defaultsProviders = new LinkedList<IResourceProvider>(defaultsProviders);
            Create(_provider, _defaultsProviders.First);
        }

        public virtual TObject Read<TObject>()
        {
            Create(_provider, _defaultsProviders.First);

            var serializer = new CompositeResourceSerializer<TObject>();
            return serializer.Deserialize(_provider.Read(), _format);
        }

        public virtual void Write<TObject>(TObject obj)
        {
            var serializer = new CompositeResourceSerializer<TObject>();
            _provider.Write(serializer.Serialize(obj, _format));
        }

        private void Create(IResourceProvider provider, LinkedListNode<IResourceProvider> altProviderNode)
        {
            var resource = provider.Read();
            if (resource == null)
            {
                var altProvider = altProviderNode?.Value;
                if (altProvider == null)
                    throw new ResourceException("Cannot read configuration");

                Create(altProvider, altProviderNode.Next);

                resource = altProvider.Read();
                provider.Write(resource);
            }
        }
    }
}
