using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AstralKeks.Workbench.Common.Content
{
    public interface IResource
    {
        bool CanRead { get; }
        bool CanWrite { get; }

        string Content { get; set; }

        TObject Read<TObject>(IResource overrides = null);
        void Write<TObject>(TObject obj);
    }

    public abstract class Resource : IResource
    {
        private readonly string _name;
        private readonly IResourceReader _reader;
        private readonly IResourceWriter _writer;

        public Resource(string name, IResourceReader reader, IResourceWriter writer)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Resource name is not set", nameof(name));

            _name = name;
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public virtual bool CanRead => _reader.CanRead(_name);

        public virtual bool CanWrite => _writer.CanWrite(_name);

        public virtual string Content
        {
            get => _reader.Read(_name);
            set => _writer.Write(_name, value);
        }

        public virtual TObject Read<TObject>(IResource overrides = null)
        {
            return default(TObject);
        }

        public virtual void Write<TObject>(TObject obj)
        {

        }
    }

    public class JsonResource : Resource
    {
        public JsonResource(string name, IResourceReader reader, IResourceWriter writer)
            : base(name, reader, writer)
        {
        }

        public JContainer Json
        {
            get => (JContainer)JsonConvert.DeserializeObject(Content);
            set => Content = JsonConvert.SerializeObject(value);
        }

        public override TObject Read<TObject>(IResource overrides = null)
        {
            var result = Json;

            var overridesContainer = (overrides as JsonResource)?.Json;
            if (overridesContainer != null)
                result.Merge(overridesContainer);

            return result.ToObject<TObject>();
        }

        public override void Write<TObject>(TObject obj)
        {
            Json = (JContainer)JToken.FromObject(obj);
        }
    }

    public class XmlResource : Resource
    {
        public XmlResource(string name, IResourceReader reader, IResourceWriter writer)
            : base(name, reader, writer)
        {
        }

        public XElement Xml
        {
            get => XElement.Parse(Content);
            set => Content = value?.ToString();
        }

        public override TObject Read<TObject>(IResource overrides = null)
        {
            var serializer = new XmlSerializer(typeof(TObject));
            using (var reader = new StringReader(Content))
            {
                return (TObject)serializer.Deserialize(reader);
            }
        }

        public override void Write<TObject>(TObject obj)
        {
            var serializer = new XmlSerializer(typeof(TObject));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                Content = writer.ToString();
            }
        }
    }
}
