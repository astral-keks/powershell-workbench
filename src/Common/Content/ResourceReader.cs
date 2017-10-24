using AstralKeks.Workbench.Common.Infrastructure;
using System;

namespace AstralKeks.Workbench.Common.Content
{
    public interface IResourceReader
    {
        bool CanRead(string resourceName);

        string Read(string resourceName);
    }

    public class EmbeddedResourceReader : IResourceReader
    {
        private readonly ResourceBundle _resourceBundle;

        public EmbeddedResourceReader(ResourceBundle resourceBundle)
        {
            _resourceBundle = resourceBundle ?? throw new ArgumentNullException(nameof(resourceBundle));
        }

        public bool CanRead(string resourceName)
        {
            return _resourceBundle.ExistsResource(resourceName);
        }

        public string Read(string resourceName)
        {
            return _resourceBundle.GetResource(resourceName);
        }
    }
}
