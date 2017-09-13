using System;

namespace AstralKeks.Workbench.Models
{
    public class Workspace
    {
        internal Workspace(string directory, string profile)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid workspace directory", nameof(directory));
            if (string.IsNullOrWhiteSpace(profile))
                throw new ArgumentException("Invalid workspace profile", nameof(profile));

            Directory = directory;
            Profile = profile;
        }

        public string Directory { get; }
        public string Profile { get; }
    }
}
