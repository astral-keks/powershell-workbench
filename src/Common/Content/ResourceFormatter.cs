using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AstralKeks.Workbench.Common.Content
{
    public interface IResourceFormatter
    {
        bool CanFormat(string resourceName);

        JToken Format(string resourceName, string resourceContent);
        string Format(string resourceName, JToken resourceJson);
    }

    public class CompositeResourceFormatter : IResourceFormatter
    {
        private readonly List<IResourceFormatter> _formatters;

        public CompositeResourceFormatter(params IResourceFormatter[] formatters)
        {
            _formatters = formatters?.ToList() ?? throw new ArgumentNullException(nameof(formatters));
        }

        public bool CanFormat(string resourceName)
        {
            return _formatters.Any(f => f.CanFormat(resourceName));
        }

        public JToken Format(string resourceName, string resourceContent)
        {
            var formatter = _formatters.FirstOrDefault(f => f.CanFormat(resourceName));
            return formatter?.Format(resourceName, resourceContent);
        }

        public string Format(string resourceName, JToken resourceJson)
        {
            var formatter = _formatters.FirstOrDefault(f => f.CanFormat(resourceName));
            return formatter?.Format(resourceName, resourceJson);
        }
    }

    public class JsonResourceFormatter : IResourceFormatter
    {
        public bool CanFormat(string resourceName)
        {
            return Path.GetExtension(resourceName)?.ToLower() == ".json";
        }

        public JToken Format(string resourceName, string resourceContent)
        {
            return resourceContent != null ? (JToken)JsonConvert.DeserializeObject(resourceContent) : null;
        }

        public string Format(string resourceName, JToken resourceJson)
        {
            return resourceJson != null ? resourceJson.ToString() : string.Empty;
        }
    }

    public class XmlResourceFormatter : IResourceFormatter
    {
        public bool CanFormat(string resourceName)
        {
            return Path.GetExtension(resourceName)?.ToLower() == ".xml";
        }


        public JToken Format(string resourceName, string resourceContent)
        {
            JToken json = null;

            if (string.IsNullOrEmpty(resourceContent))
            {
                var xml = XElement.Parse(resourceContent);
                var jsonContent = JsonConvert.SerializeXNode(xml);
                json = JToken.Parse(jsonContent);
            }

            return json;
        }

        public string Format(string resourceName, JToken resourceJson)
        {
            string resourceContent = null;

            if (resourceJson != null)
            {
                var jsonContent = resourceJson.ToString();
                var xml = JsonConvert.DeserializeXNode(jsonContent);
                resourceContent = xml.ToString();
            }

            return resourceContent;
        }
    }
}
