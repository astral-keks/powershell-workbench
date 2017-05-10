
using System.Collections.Generic;

namespace AstralKeks.Workbench.Core.Data
{
    public class History
    {
        public History(HistoryType type)
        {
            Type = type;
            Entries = new List<string>();
        }

        public HistoryType Type { get; set; }
        public List<string> Entries { get; set; }
    }

    public enum HistoryType
    {
        Workspace
    }
}
