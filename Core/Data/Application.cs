using System.Collections.Generic;

namespace AstralKeks.Workbench.Core.Data
{
    public class Application
    {
        public string Name { get; set; }
        public string Executable { get; set; }
        public List<Command> Commands { get; set; }
    }
}
