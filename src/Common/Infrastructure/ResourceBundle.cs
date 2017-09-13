using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AstralKeks.Workbench.Common.Infrastructure
{
    public class ResourceBundle
    {
        public bool ExistsResource(string resourceName, Type assemblyLocator)
        {
            return GetResourceName(resourceName, assemblyLocator) != null;
        }

        public string GetResource(string resourceName, Type assemblyLocator)
        {
            string content = null;

            resourceName = GetResourceName(resourceName, assemblyLocator);
            var assembly = assemblyLocator.GetTypeInfo().Assembly;
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


        private string GetResourceName(string resourceName, Type assemblyLocator)
        {
            var platform = GetPlatform();
            var assembly = assemblyLocator.GetTypeInfo().Assembly;
            var resourceQuery = $"{assemblyLocator.Namespace}.Resources.{platform}.{resourceName}";

            var resourceNames = assembly.GetManifestResourceNames();
            resourceName = resourceNames
                .Where(r => r.Contains(platform))
                .FirstOrDefault(r => r == resourceQuery);
            if (resourceName == null)
            {
                resourceName = resourceNames
                    .Where(r => r.Contains(platform))
                    .FirstOrDefault(r => Regex.IsMatch(resourceQuery, r));
            }

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
