using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace AstralKeks.Workbench.Core.Resources
{
    public interface IResourceFormat
    {
        TObject Deserialize<TObject>(string content);

        string Serialize<TObject>(TObject obj);
    }

    public class JsonResourceFormat : IResourceFormat
    {
        public TObject Deserialize<TObject>(string content)
        {
            var jtoken = (JToken)JsonConvert.DeserializeObject(content);
            return jtoken.ToObject<TObject>();
        }

        public string Serialize<TObject>(TObject obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }

    public class XmlResourceFormat : IResourceFormat
    {
        public TObject Deserialize<TObject>(string content)
        {
            var xNode = XElement.Parse(content);
            var jsonContent = JsonConvert.SerializeXNode(xNode);
            var jtoken = (JToken)JsonConvert.DeserializeObject(jsonContent);
            return jtoken.ToObject<TObject>();
        }

        public string Serialize<TObject>(TObject obj)
        {
            var jsonContent = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var xNode = JsonConvert.DeserializeXNode(jsonContent);
            return xNode.ToString();
        }
    }
}
