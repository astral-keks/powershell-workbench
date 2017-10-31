using System.Collections.Generic;

namespace AstralKeks.Workbench.Configuration
{
    internal class ShortcutConfig
    {
        public List<ShortcutPattern> Dynamic { get; set; } = new List<ShortcutPattern>();

        public List<ShortcutPattern> Synchronized { get; set; } = new List<ShortcutPattern>();
    }

    internal class ShortcutPattern
    {
        public string RootDirectory { get; set; }

        public string Location { get; set; }

        public string PathPattern { get; set; }

        public bool SearchRecursively { get; set; } = true;

        public List<string> InnerPathPatterns { get; set; } = new List<string>();
    }
}
