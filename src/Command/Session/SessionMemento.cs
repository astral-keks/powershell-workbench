using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    internal class SessionMemento
    {
        public static Queue<SessionMemento> Saved { get; } = new Queue<SessionMemento>();

        public SessionMemento(string directory, string location, string modulePath, List<PSModuleInfo> modules, List<AliasInfo> aliases)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid working directory", nameof(directory));
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Invalid location", nameof(location));

            Directory = directory;
            Location = location;
            ModulePath = modulePath ?? string.Empty;
            Modules = modules ?? throw new System.ArgumentNullException(nameof(modules));
            Aliases = aliases ?? throw new System.ArgumentNullException(nameof(aliases));
        }

        public string Directory { get; }
        public string Location { get; }
        public string ModulePath { get; }
        public List<PSModuleInfo> Modules { get; }
        public List<AliasInfo> Aliases { get; }
    }
}
