using System;

namespace AstralKeks.Workbench.Core.Data
{
    public class Workspace
    {
        public const string Default = "Default";

        public Workspace(string name, string[] directories, string[] files)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Directories = directories ?? throw new ArgumentNullException(nameof(directories));
            Files = files ?? throw new ArgumentNullException(nameof(files));
        }

        public string Name { get; }

        public string[] Directories { get; }

        public string[] Files { get; }
    }
}
