using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace AstralKeks.Workbench.Core.Resource
{
    public interface IResourceFormat<TObject>
    {
        TObject Deserialize(string content);

        string Serialize(TObject obj);
    }

    public class StringResourceFormat : IResourceFormat<string>
    {
        public string Deserialize(string content)
        {
            return content;
        }

        public string Serialize(string obj)
        {
            return obj;
        }
    }

    public class JsonResourceFormat<TObject> : IResourceFormat<TObject>
    {
        public TObject Deserialize(string content)
        {
            var jtoken = (JToken)JsonConvert.DeserializeObject(content);
            return jtoken.ToObject<TObject>();
        }

        public string Serialize(TObject obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }

    public class XmlResourceFormat<TObject> : IResourceFormat<TObject>
    {
        public TObject Deserialize(string content)
        {
            var xNode = XElement.Parse(content);
            var jsonContent = JsonConvert.SerializeXNode(xNode);
            var jtoken = (JToken)JsonConvert.DeserializeObject(jsonContent);
            return jtoken.ToObject<TObject>();
        }

        public string Serialize(TObject obj)
        {
            var jsonContent = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var xNode = JsonConvert.DeserializeXNode(jsonContent);
            return xNode.ToString();
        }
    }
}
