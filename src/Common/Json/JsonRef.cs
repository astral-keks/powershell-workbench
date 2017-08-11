using System;
using Newtonsoft.Json.Linq;

namespace AstralKeks.Workbench.Common.Json
{
    public class JsonRef : IJsonObject
    {
        private readonly IJsonObject _parent;
        private readonly string _refPath;

        public JsonRef(IJsonObject parent, string refPath) 
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));
            if (refPath == null)
                throw new ArgumentNullException(nameof(refPath));

            _parent = parent;
            _refPath = refPath;
        }

        public TObject Read<TObject>()
        {
            var parentJson = _parent.Read<JToken>();
            return (TObject)(object)parentJson.SelectToken(GetPath(parentJson, _refPath));
        }

        public void Write<TObject>(TObject obj)
        {
            var parentJson = _parent.Read<JToken>();
            parentJson.SelectToken(GetPath(parentJson, _refPath)).Replace((JToken)(object)obj);
            _parent.Write(parentJson);
        }

        public void Remove()
        {
            var parentJson = _parent.Read<JToken>();
            parentJson.SelectToken(GetPath(parentJson, _refPath)).Remove();
            _parent.Write(parentJson);
        }

        private string GetPath(JToken parent, string path)
        {
            if (path.StartsWith(parent.Path))
                path = path.Substring(parent.Path.Length);
            return path;
        }
    }
}
