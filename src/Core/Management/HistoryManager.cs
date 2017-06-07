using AstralKeks.Workbench.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AstralKeks.Workbench.Core.Management
{
    public class HistoryManager
    {
        private const int _historyMaxSize = 10;
        private readonly UserspaceManager _userspaceManager;
        private readonly ResourceManager _resourceManager;

        public HistoryManager(UserspaceManager userspaceManager, ResourceManager resourceManager)
        {
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        }

        public void AppendHistoryEntry(string entry, HistoryType historyType)
        {
            if (entry != null)
            {
                var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
                var historyResource = _resourceManager.GetResource<List<History>>(userspaceDirectory,
                    FileSystem.ConfigDirectory, FileSystem.HistoryFile);
                var histories = historyResource.Read();
                var history = histories.FirstOrDefault(h => h.Type == historyType);
                if (history == null)
                {
                    history = new History(historyType);
                    histories.Add(history);
                }

                var historyEntries = new List<string> { entry };
                foreach (var existingEntry in history.Entries)
                {
                    if (!string.Equals(entry, existingEntry, StringComparison.OrdinalIgnoreCase))
                        historyEntries.Add(existingEntry);
                }
                if (historyEntries.Count > _historyMaxSize)
                    historyEntries.RemoveAt(historyEntries.Count - 1);

                history.Entries.Clear();
                history.Entries.AddRange(historyEntries);

                historyResource.Write(histories);
            }
        }

        public string[] GetHistoryEntries(HistoryType historyType)
        {
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            var historyResource = _resourceManager.GetResource<History[]>(userspaceDirectory,
                   FileSystem.ConfigDirectory, FileSystem.HistoryFile);
            var histories = historyResource.Read();
            return histories.FirstOrDefault(h => h.Type == historyType)?.Entries.ToArray() ?? new string[0];
        }
    }
}
