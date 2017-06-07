
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

        public HistoryType Type { get; }
        public List<string> Entries { get; }
    }

    public enum HistoryType
    {
        Workspace
    }
}
