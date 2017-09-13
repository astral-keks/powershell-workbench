using System;

namespace AstralKeks.Workbench.Common.Resources
{
    public class ResourceOrigin
    {
        public ResourceOrigin(Type assemblyLocator)
        {
            AssemblyLocator = assemblyLocator ?? throw new ArgumentNullException(nameof(assemblyLocator));
        }

        public Type AssemblyLocator { get; }
    }
}
