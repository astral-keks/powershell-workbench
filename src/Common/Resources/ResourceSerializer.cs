using System;
using System.Collections.Generic;
using System.Linq;

namespace AstralKeks.Workbench.Common.Resources
{
    public interface IResourceSerializer<TObject>
    {
        string Serialize(TObject obj, IResourceFormat format);
        TObject Deserialize(string content, IResourceFormat format);
    }

    public class CompositeResourceSerializer<TObject> : IResourceSerializer<TObject>
    {
        private readonly IList<object> _serializers = new object[] 
        {
            new StringResourceSerializer(),
            new GenericResourceSerializer<TObject>()
        };

        public TObject Deserialize(string content, IResourceFormat format)
        {
            var serializer = GetSerializer();
            return serializer.Deserialize(content, format);
        }

        public string Serialize(TObject obj, IResourceFormat format)
        {
            var serializer = GetSerializer();
            return serializer.Serialize(obj, format);
        }

        private IResourceSerializer<TObject> GetSerializer()
        {
            var serializer = _serializers.FirstOrDefault(s => s is IResourceSerializer<TObject>);
            if (serializer == null)
                throw new InvalidOperationException($"Serializer for {typeof(TObject).FullName} was not found");
            return serializer as IResourceSerializer<TObject>;
        }
    }

    public class StringResourceSerializer : IResourceSerializer<string>
    {
        public string Deserialize(string content, IResourceFormat format)
        {
            return content;
        }

        public string Serialize(string obj, IResourceFormat format)
        {
            return obj;
        }
    }

    public class GenericResourceSerializer<TObject> : IResourceSerializer<TObject>
    {
        public TObject Deserialize(string content, IResourceFormat format)
        {
            return format.Deserialize<TObject>(content);
        }

        public string Serialize(TObject obj, IResourceFormat format)
        {
            return format.Serialize(obj);
        }
    }
}