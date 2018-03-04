using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AstralKeks.Workbench.Common.Infrastructure
{
    public class ResourceBundle
    {
        private readonly Type _assemblyLocator;

        public ResourceBundle(Type assemblyLocator)
        {
            _assemblyLocator = assemblyLocator ?? throw new ArgumentNullException(nameof(assemblyLocator));
        }

        public bool ExistsResource(string resourceName)
        {
            return FindResourceName(resourceName) != null;
        }

        public string GetResource(string resourceName)
        {
            string content = null;

            resourceName = FindResourceName(resourceName);
            var assembly = _assemblyLocator.GetTypeInfo().Assembly;
            if (!string.IsNullOrWhiteSpace(resourceName))
            {
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                            content = reader.ReadToEnd();
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot find resource {resourceName}");
                    }
                }
            }
            return content;
        }
        
        private string FindResourceName(string resourceName)
        {
            var platform = GetPlatform();
            var platformResourceQuery = $"{_assemblyLocator.Namespace}.Resources.{platform}.{resourceName}";
            var commonResourceQuery = $"{_assemblyLocator.Namespace}.Resources.Common.{resourceName}";
            return GetResourceName(platformResourceQuery) ?? GetResourceName(commonResourceQuery);
        }

        private string GetResourceName(string resourceQuery)
        {
            var assembly = _assemblyLocator.GetTypeInfo().Assembly;
            var resourceNames = assembly.GetManifestResourceNames();
            var resourceName = resourceNames.FirstOrDefault(r => r == resourceQuery);
            if (resourceName == null)
                resourceName = resourceNames.FirstOrDefault(r => Regex.IsMatch(resourceQuery, r));

            return resourceName;
        }


        private string GetPlatform()
        {
            switch (Path.DirectorySeparatorChar)
            {
                case FileSystem.WindowsSeparator:
                    return "Windows";
                case FileSystem.UnixSeparator:
                    return "Unix";
                default:
                    throw new NotSupportedException("Unsupported platfrom");
            }
        }
    }
}
