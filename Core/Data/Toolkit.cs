using System;

namespace AstralKeks.Workbench.Core.Data
{
    public class Toolkit
    {
        public Toolkit(string directory, string module)
        {
            Directory = directory ?? throw new ArgumentNullException(nameof(directory));
            Module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public string Directory { get; }

        public string Module { get; }
    }
}
