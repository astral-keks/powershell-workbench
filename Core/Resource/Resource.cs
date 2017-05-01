using System;
using System.Collections.Generic;
using System.Linq;

namespace AstralKeks.Workbench.Core.Resource
{
    public class Resource<TObject>
    {
        private readonly IResourceFormat<TObject> _format;
        private readonly IResourceProvider _provider;
        private readonly LinkedList<IResourceProvider> _defaultsProviders;

        public Resource(IResourceFormat<TObject> format, IResourceProvider provider, params IResourceProvider[] defaultsProviders)
        {
            if (!defaultsProviders.Any())
                throw new ArgumentException("No configuration providers have been specified", nameof(defaultsProviders));

            _format = format ?? throw new ArgumentNullException(nameof(format));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _defaultsProviders = new LinkedList<IResourceProvider>(defaultsProviders);
        }

        public virtual TObject Read()
        {
            var configuration = Read(_provider, _defaultsProviders.First);
            return _format.Deserialize(configuration);
        }

        public virtual void Write(TObject obj)
        {
            var resource = _format.Serialize(obj);
            _provider.Write(resource);
        }

        private string Read(IResourceProvider provider, LinkedListNode<IResourceProvider> altProviderNode)
        {
            var resource = provider.Read();
            if (resource == null)
            {
                if (altProviderNode == null)
                    throw new ConfigurationException("Cannot read configuration");

                resource = Read(altProviderNode.Value, altProviderNode.Next);
                provider.Write(resource);
            }

            return resource;
        }
    }

    [Serializable]
    public class ConfigurationException : Exception
    {
        public ConfigurationException() { }
        public ConfigurationException(string message) : base(message) { }
        public ConfigurationException(string message, Exception inner) : base(message, inner) { }
    }
}
