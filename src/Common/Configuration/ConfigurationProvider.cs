using AstralKeks.Workbench.Common.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AstralKeks.Workbench.Common.Configuration
{
    public class ConfigurationProvider
    {
        private readonly ResourceManager _resourceManager;

        public ConfigurationProvider(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        }

        public TConfig Get<TConfig>(string defaultsPath, string overridesPath = null) 
            where TConfig : class
        {
            if (string.IsNullOrWhiteSpace(defaultsPath))
                throw new ArgumentException("Defaults path is not set", nameof(defaultsPath));

            var fileName = Path.GetFileName(defaultsPath);
            var configResource = _resourceManager.CreateResource(new[] { defaultsPath }, fileName);
            var overridingConfigResource = !string.IsNullOrWhiteSpace(overridesPath)
                ? _resourceManager.GetResource(new[] { overridesPath })
                : null;

            var config = configResource.Read<TConfig>();
            var overridingConfig = overridingConfigResource?.Read<TConfig>();
            return overridingConfig != null ? MergeResources(config, overridingConfig) : config;
        }

        private TConfig MergeResources<TConfig>(TConfig config, TConfig overridingConfig = null) 
            where TConfig : class
        {
            var valueProperties = typeof(TConfig)
                .GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(ConfigurationValueAttribute)));
            foreach (var property in valueProperties)
            {
                var value = property.GetValue(config);
                var overridingValue = property.GetValue(overridingConfig);

                var attribute = property
                    .GetCustomAttributes(typeof(ConfigurationValueAttribute), true)
                    .Cast<ConfigurationValueAttribute>()
                    .Single();
                switch (attribute.Behavior)
                {
                    case ConfigurationValueBehavior.Merge:
                        property.SetValue(config, Union(value, overridingValue, property));
                        break;
                    case ConfigurationValueBehavior.Override:
                        property.SetValue(config, overridingValue);
                        break;
                    case ConfigurationValueBehavior.Keep:
                        break;
                }
            }

            return config;
        }

        private object Union(object list1, object list2, PropertyInfo property)
        {
            var itemType = property.PropertyType.GetGenericArguments().FirstOrDefault();
            if (property.PropertyType == typeof(List<>) && itemType != null)
            {
                var unionMethod = typeof(Enumerable)
                    .GetMethods()
                    .FirstOrDefault(m => m.Name == "Union")
                    .MakeGenericMethod(itemType);
                var union = unionMethod.Invoke(null, new[] { list1, list2 });

                var toListMethod = typeof(Enumerable)
                    .GetMethods()
                    .FirstOrDefault(m => m.Name == "ToList")
                    .MakeGenericMethod(itemType);
                list1 = toListMethod.Invoke(null, new[] { union }); 
            }

            return list1;
        }
    }
}
