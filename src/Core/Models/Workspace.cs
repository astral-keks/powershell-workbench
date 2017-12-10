using System;
using System.Linq;

namespace AstralKeks.Workbench.Models
{
    public class Workspace
    {
        internal Workspace(string directory, string marker, string[] profiles)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid workspace directory", nameof(directory));
            if (string.IsNullOrWhiteSpace(marker))
                throw new ArgumentException("Invalid workspace marker path", nameof(marker));
            if (profiles == null)
                throw new ArgumentNullException(nameof(profiles));

            Directory = directory;
            Profiles = profiles;
            Marker = marker;
        }

        public string Directory { get; }
        public string[] Profiles { get; }
        public string Marker { get; }
    }
}
