using System;
using System.Collections.Generic;
using System.Linq;

namespace AstralKeks.Workbench.Common.Resources
{
    public class Resource
    {
        private readonly IResourceFormat _format;
        private readonly IResourceProvider _provider;

        public Resource(IResourceFormat format, IResourceProvider provider)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            _format = format ?? throw new ArgumentNullException(nameof(format));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public bool Exists => _provider.CanRead;

        public TObject Read<TObject>()
        {
            var serializer = new CompositeResourceSerializer<TObject>();
            return serializer.Deserialize(_provider.Read(), _format);
        }

        public void Write<TObject>(TObject obj)
        {
            var serializer = new CompositeResourceSerializer<TObject>();
            _provider.Write(serializer.Serialize(obj, _format));
        }
    }
}
