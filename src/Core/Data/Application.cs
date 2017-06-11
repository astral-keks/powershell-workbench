using System.Collections.Generic;

namespace AstralKeks.Workbench.Core.Data
{
    public class Application
    {
        public const string Default = "Terminal";
        public const string Editor = "Editor";

        public string Name { get; set; }
        public string Executable { get; set; }
        public List<Command> Commands { get; set; }
    }
}
