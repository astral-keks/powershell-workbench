using System;

namespace AstralKeks.Workbench.Core.Data
{
    public class WorkspaceTemplate
    {
        public const string Default = "Default";

        public string Name { get; set; }
        public string[] Directories { get; set; }
        public string[] Files { get; set; }
    }
}
