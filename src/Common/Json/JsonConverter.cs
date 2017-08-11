using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AstralKeks.Workbench.Common.Json
{
    public static class JsonConverter
    {
        private delegate object Constructor(JToken json, IJsonObject parent);
        private static readonly Dictionary<JTokenType, Constructor> _constructors = new Dictionary<JTokenType, Constructor>
        {
            {JTokenType.Boolean, (json, parent) => json.ToObject(typeof(bool))},
            {JTokenType.Float, (json, parent) => json.ToObject(typeof(decimal))},
            {JTokenType.String, (json, parent) => json.ToObject(typeof(string))},
            {JTokenType.Integer, (json, parent) => json.ToObject(typeof(long))},
            {JTokenType.Date, (json, parent) => json.ToObject(typeof(DateTime))},
            {JTokenType.TimeSpan, (json, parent) => json.ToObject(typeof(TimeSpan))},
            {JTokenType.Array, (json, parent) => new JsonList(new JsonRef(parent, json.Path))},
            {JTokenType.Object, (json, parent) => new JsonMap(new JsonRef(parent, json.Path))},
            {JTokenType.Null, (json, parent) => null}
        };

        public static JToken ToJson(this object value)
        {
            return JToken.FromObject(value);
        }

        public static object ToObject(this JToken json, IJsonObject parent)
        {
            if (json == null)
                return null;
            var constructor = _constructors.ContainsKey(json.Type) ? _constructors[json.Type] : null;
            return constructor?.Invoke(json, parent);
        }

    }
}
