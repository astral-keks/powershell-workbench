using System;

namespace AstralKeks.Workbench.Common.Configuration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConfigurationValueAttribute : Attribute
    {
        public ConfigurationValueAttribute()
        {
        }

        public ConfigurationValueAttribute(ConfigurationValueBehavior behavior)
        {
            Behavior = behavior;
        }

        public ConfigurationValueBehavior Behavior { get; }
    }
}
