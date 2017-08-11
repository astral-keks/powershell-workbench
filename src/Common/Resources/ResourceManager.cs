using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.FileSystem;
using System;
using System.Collections.Generic;
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

        public Resource CreateResource(string location, string directoryName, string fileName)
        {
            return CreateResource(new[] { location }, directoryName, fileName);
        }

        public Resource CreateResource(string location, string fileName)
        {
            return CreateResource(new[] { location }, fileName);
        }

        public Resource CreateResource(IEnumerable<string> locations, string directoryName, string fileName)
        {
            if (locations == null)
                throw new ArgumentNullException(nameof(locations));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Invalid name of file", nameof(fileName));
            locations = locations.Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

            var mainLocation = locations.FirstOrDefault();
            var defaultLocations = locations.Skip(1).ToList();
            if (mainLocation == null)
                throw new ArgumentException("Location is not set");

            var mainPath = new ResourceLocator(mainLocation, directoryName, fileName).Path;
            var defaultPaths = defaultLocations.Select(l => new ResourceLocator(l, directoryName, fileName).Path).ToList();
            var defaultResourceName = GetResourceName(fileName);

            var mainProvider = new FileResourceProvider(mainPath);
            var defaultProviders = defaultPaths.Select(p => new FileResourceProvider(p)).Cast<IResourceProvider>().ToList();
            defaultProviders.Add(new EmbeddedResourceProvider(defaultResourceName, _assembly));

            return new Resource(_format, mainProvider, defaultProviders);
        }

        public Resource CreateResource(IEnumerable<string> locations, string fileName)
        {
            if (locations == null)
                throw new ArgumentNullException(nameof(locations));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Invalid name of file", nameof(fileName));
            locations = locations.Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

            var mainLocation = locations.FirstOrDefault();
            var defaultLocations = locations.Skip(1).ToList();
            if (mainLocation == null)
                throw new ArgumentException("Location is not set");

            var mainPath = new ResourceLocator(mainLocation, fileName).Path;
            var defaultPaths = defaultLocations.Select(l => new ResourceLocator(l, fileName).Path).ToList();
            var defaultResourceName = GetResourceName(fileName);

            var mainProvider = new FileResourceProvider(mainPath);
            var defaultProviders = defaultPaths.Select(p => new FileResourceProvider(p)).Cast<IResourceProvider>().ToList();
            defaultProviders.Add(new EmbeddedResourceProvider(defaultResourceName, _assembly));

            return new Resource(_format, mainProvider, defaultProviders);
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
