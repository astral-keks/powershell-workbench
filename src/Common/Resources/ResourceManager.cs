using AstralKeks.Workbench.Common.Data;
using AstralKeks.Workbench.Common.FileSystem;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AstralKeks.Workbench.Common.Resources
{
    public class ResourceManager
    {
        private readonly Assembly _assembly;
        private readonly string _namespace;
        private readonly IResourceFormat _format;

        public ResourceManager(Type type, IResourceFormat format = null)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            _assembly = type.GetTypeInfo().Assembly;
            _namespace = $"{type.Namespace}.Resources";
            _format = format ?? new JsonResourceFormat();
        }

        public ResourceManager(Assembly assembly, string @namespace, IResourceFormat format = null)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            if (string.IsNullOrEmpty(@namespace))
                throw new ArgumentException("Invalid namespace");

            _assembly = assembly;
            _namespace = @namespace;
            _format = format ?? new JsonResourceFormat();
        }

        public Resource ObtainResource(string workspaceLocation, string userspaceLocation,
            string directoryName, string fileName)
        {
            var workspacePath = new ResourceLocator(workspaceLocation, directoryName, fileName).Path;
            var userspacePath = new ResourceLocator(userspaceLocation, directoryName, fileName).Path;
            var embeddedResourceName = GetResourceName(fileName);

            var workspaceProvider = new FileResourceProvider(workspacePath);
            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName, _assembly);
            
            return new Resource(_format, workspaceProvider, userspaceProvider, defaults);
        }

        public Resource ObtainResource(string userspaceLocation, string directoryName, string fileName)
        {
            var userspacePath = new ResourceLocator(userspaceLocation, directoryName, fileName).Path;
            var embeddedResourceName = GetResourceName(fileName);

            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName, _assembly);

            return new Resource(_format, userspaceProvider, defaults);
        }

        public Resource ObtainResource(string location, string fileName)
        {
            var userspacePath = new ResourceLocator(location, fileName).Path;
            var embeddedResourceName = GetResourceName(fileName);

            var userspaceProvider = new FileResourceProvider(userspacePath);
            var defaults = new EmbeddedResourceProvider(embeddedResourceName, _assembly);

            return new Resource(_format, userspaceProvider, defaults);
        }

        public void DeleteResource(string location, string directoryName, string fileName)
        {
            var path = new ResourceLocator(location, directoryName, fileName).Path;
            FsOperation.DeleteFile(path);
        }

        public void DeleteResource(string location, string fileName)
        {
            var path = new ResourceLocator(location, fileName).Path;
            FsOperation.DeleteFile(path);
        }

        public string GetResourceName(string fileName)
        {
            var resourceQuery = $"{_namespace}.{Platform.Current}.{fileName}";

            var resourceNames = _assembly.GetManifestResourceNames();
            var resourceName = resourceNames
                .Where(r => r.Contains(Platform.Current))
                .FirstOrDefault(r => r == resourceQuery);
            if (resourceName == null)
            {
                resourceName = resourceNames
                    .Where(r => r.Contains(Platform.Current))
                    .FirstOrDefault(r => Regex.IsMatch(resourceQuery, r));
            }
            if (resourceName == null)
                throw new ArgumentException($"Embedded resource {fileName} was not found");

            return resourceName;
        }
    }
}
