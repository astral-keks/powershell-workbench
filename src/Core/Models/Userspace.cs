using System;

namespace AstralKeks.Workbench.Models
{
    public class Userspace
    {
        internal Userspace(string name, string directory, string profile)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Invalid userspace name", nameof(name));
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid userspace directory", nameof(directory));
            if (string.IsNullOrWhiteSpace(profile))
                throw new ArgumentException("Invalid userspace profile", nameof(profile));

            Name = name;
            Directory = directory;
            Profile = profile;
        }

        public string Name { get; }
        public string Directory { get; }
        public string Profile { get; }
    }
}
