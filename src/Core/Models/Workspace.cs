using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Resources;
using System;

namespace AstralKeks.Workbench.Models
{
    public class Workspace
    {
        internal Workspace(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("Invalid workspace directory", nameof(directory));

            Directory = directory;
            Profile = PathBuilder.Combine(directory, Files.WorkspacePs1);
        }

        public string Directory { get; }
        public string Profile { get; }
    }
}
