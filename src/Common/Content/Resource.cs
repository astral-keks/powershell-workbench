using Newtonsoft.Json.Linq;
using System;

namespace AstralKeks.Workbench.Common.Content
{
    public interface IResource
    {
        string Name { get; }

        bool CanRead { get; }
        bool CanWrite { get; }

        string Content { get; set; }
        JToken Json { get; set; }

        TObject Read<TObject>(IResource overrides = null);
        void Write<TObject>(TObject obj);
    }

    public class Resource : IResource
    {
        private readonly string _name;
        private readonly IResourceReader _reader;
        private readonly IResourceWriter _writer;
        private readonly IResourceFormatter _formatter;

        public Resource(string name, string content) : this (name, new MemoryResourceProvider(name, content))
        {
        }

        public Resource(string name, IResourceProvider provider) : this(name, provider, provider)
        {
        }

        public Resource(string name, IResourceReader reader, IResourceWriter writer)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Resource name is not set", nameof(name));

            _name = name;
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _formatter = new CompositeResourceFormatter(
                new JsonResourceFormatter(), 
                new XmlResourceFormatter());
        }

        public virtual string Name => _name;

        public virtual bool CanRead => _reader.CanRead(_name);

        public virtual bool CanWrite => _writer.CanWrite(_name);

        public virtual string Content
        {
            get => _reader.Read(_name);
            set => _writer.Write(_name, value);
        }

        public virtual JToken Json
        {
            get => _formatter.Format(_name, Content);
            set => Content = _formatter.Format(_name, value);
        }

        public virtual TObject Read<TObject>(IResource overrides = null)
        {
            var json = Json;
            var overridesJson = overrides?.Json;
            if (json is JContainer && overridesJson is JContainer)
                (json as JContainer).Merge((overridesJson as JContainer));

            return json != null ? json.ToObject<TObject>() : default(TObject);
        }

        public virtual void Write<TObject>(TObject obj)
        {
            Json = JToken.FromObject(obj);
        }
    }
}
