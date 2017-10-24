using AstralKeks.Workbench.Models;
using System.Collections.Generic;

namespace AstralKeks.Workbench.Configuration
{
    internal class ShortcutConfig
    {
        public List<Shortcut> Static { get; set; } = new List<Shortcut>();

        public List<ShortcutPattern> Dynamic { get; set; } = new List<ShortcutPattern>();
    }

    internal class ShortcutPattern
    {
        public string RootDirectory { get; set; }

        public string Location { get; set; }

        public string PathPattern { get; set; }

        public List<string> InnerPathPatterns { get; set; } = new List<string>();
    }
}
