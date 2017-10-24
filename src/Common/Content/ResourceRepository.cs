using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Template;

namespace AstralKeks.Workbench.Common.Content
{
    public class ResourceRepository
    {
        private readonly IResourceFormatter _resourceFormatter;
        private readonly IResourceReader _defaultResourceReader;
        private readonly IResourceProvider _rawResourceProvider;
        private readonly IResourceProvider _resourceProvider;

        public ResourceRepository(ResourceBundle resourceBundle, FileSystem fileSystem, TemplateProcessor templateProcessor)
        {
            _resourceFormatter = new CompositeResourceFormatter(new JsonResourceFormatter(), new XmlResourceFormatter());
            _defaultResourceReader = new EmbeddedResourceReader(resourceBundle);
            _rawResourceProvider = new FileResourceProvider(fileSystem);
            _resourceProvider = new TemplateResourceProvider(_rawResourceProvider, templateProcessor);
        }

        public IResource CreateResource(string resourcePath, string resourceName)
        {
            if (_rawResourceProvider.CanWrite(resourcePath) && _defaultResourceReader.CanRead(resourceName))
            {
                var resourceContent = _defaultResourceReader.Read(resourceName);
                _rawResourceProvider.Write(resourcePath, resourceContent);
            }

            return GetResource(resourcePath);
        }

        public IResource GetResource(string resourcePath)
        {
            return _resourceProvider.CanRead(resourcePath) 
                ? _resourceFormatter.Format(resourcePath, _resourceProvider, _resourceProvider)
                : null;
        }
    }
}
