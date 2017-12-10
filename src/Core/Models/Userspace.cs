using System;
using System.IO;

namespace AstralKeks.Workbench.Models
{
    public class Userspace
    {
        internal Userspace(string directory, string marker, string[] profiles)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid userspace directory", nameof(directory));
            if (string.IsNullOrWhiteSpace(marker))
                throw new ArgumentException("Invalid userspace marker path", nameof(marker));
            if (profiles == null)
                throw new ArgumentNullException(nameof(profiles));

            Marker = marker;
            Directory = directory;
            Profiles = profiles;

            Name = Path.GetFileName(directory);
        }

        public string Name { get; }
        public string Directory { get; }
        public string[] Profiles { get; }
        public string Marker { get; }
    }
}
