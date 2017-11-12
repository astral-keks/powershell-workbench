using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Resources;
using System;
using System.IO;

namespace AstralKeks.Workbench.Models
{
    public class Userspace
    {
        internal Userspace(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid userspace directory", nameof(directory));

            Directory = directory;
            Name = Path.GetFileName(directory);
            Profile = PathBuilder.Complete(directory, Files.UserspacePs1);
        }

        public string Name { get; }
        public string Directory { get; }
        public string Profile { get; }
    }
}
