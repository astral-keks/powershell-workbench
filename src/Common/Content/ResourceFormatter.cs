using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.Workbench.Common.Content
{
    public interface IResourceFormatter
    {
        bool CanFormat(string resourceName);

        IResource Format(string resourceName, IResourceReader resourceReader, IResourceWriter resourceWriter);
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

        public IResource Format(string resourceName, IResourceReader resourceReader, IResourceWriter resourceWriter)
        {
            return _formatters.FirstOrDefault(f => f.CanFormat(resourceName))?.Format(resourceName, resourceReader, resourceWriter);
        }
    }

    public class JsonResourceFormatter : IResourceFormatter
    {
        public bool CanFormat(string resourceName)
        {
            return Path.GetExtension(resourceName)?.ToLower() == ".json";
        }

        public IResource Format(string resourceName, IResourceReader resourceReader, IResourceWriter resourceWriter)
        {
            return new JsonResource(resourceName, resourceReader, resourceWriter);
        }
    }

    public class XmlResourceFormatter : IResourceFormatter
    {
        public bool CanFormat(string resourceName)
        {
            return Path.GetExtension(resourceName)?.ToLower() == ".xml";
        }

        public IResource Format(string resourceName, IResourceReader resourceReader, IResourceWriter resourceWriter)
        {
            return new XmlResource(resourceName, resourceReader, resourceWriter);
        }
    }
}
