using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AstralKeks.Workbench.Repositories
{
    public class ShortcutRepository
    {
        private readonly WorkspaceContext _workspaceContext;
        private readonly UserspaceContext _userspaceContext;
        private readonly FileSystem _fileSystem;

        public ShortcutRepository(WorkspaceContext workspaceContext, UserspaceContext userspaceContext, FileSystem fileSystem)
        {
            _workspaceContext = workspaceContext ?? throw new ArgumentNullException(nameof(workspaceContext));
            _userspaceContext = userspaceContext ?? throw new ArgumentNullException(nameof(userspaceContext));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public void AddShortcuts(IEnumerable<Shortcut> workspaceShortcuts = null, IEnumerable<Shortcut> userspaceShortcuts = null)
        {
            var shortcutWorkspaceStoragePath = PathBuilder.Complete(
                _workspaceContext.CurrentWorkspaceDirectory, Directories.Temp, Directories.Workbench, Files.ShortcutsTxt);
            var shortcutUserspaceStoragePath = PathBuilder.Complete(
                _userspaceContext.CurrentUserspaceDirectory, Directories.Temp, Directories.Workbench, Files.ShortcutsTxt);
            
            WriteShortcuts(shortcutWorkspaceStoragePath, workspaceShortcuts, true);
            WriteShortcuts(shortcutUserspaceStoragePath, userspaceShortcuts, true);
        }

        public void ClearShortcuts()
        {
            var shortcutWorkspaceStoragePath = PathBuilder.Complete(
                _workspaceContext.CurrentWorkspaceDirectory, Directories.Temp, Directories.Workbench, Files.ShortcutsTxt);
            var shortcutUserspaceStoragePath = PathBuilder.Complete(
                _userspaceContext.CurrentUserspaceDirectory, Directories.Temp, Directories.Workbench, Files.ShortcutsTxt);

            WriteShortcuts(shortcutWorkspaceStoragePath, Enumerable.Empty<Shortcut>(), false);
            WriteShortcuts(shortcutUserspaceStoragePath, Enumerable.Empty<Shortcut>(), false);
        }

        public IEnumerable<Shortcut> FindShortcut(string query = null)
        {
            var shortcutWorkspaceStoragePath = PathBuilder.Complete(
                _workspaceContext.CurrentWorkspaceDirectory, Directories.Temp, Directories.Workbench, Files.ShortcutsTxt);
            var shortcutUserspaceStoragePath = PathBuilder.Complete(
                _userspaceContext.CurrentUserspaceDirectory, Directories.Temp, Directories.Workbench, Files.ShortcutsTxt);

            var foundShortcuts = new HashSet<string>();
            foreach (var shortcut in ReadShortcuts(shortcutWorkspaceStoragePath).Where(s => IsShortcutMatch(s, query)))
            {
                if (foundShortcuts.Add(shortcut.ToString()))
                    yield return shortcut;
            }
            foreach (var shortcut in ReadShortcuts(shortcutUserspaceStoragePath).Where(s => IsShortcutMatch(s, query)))
            {
                if (foundShortcuts.Add(shortcut.ToString()))
                    yield return shortcut;
            }
        }

        private bool IsShortcutMatch(Shortcut shortcut, string query = null)
        {
            Func<string, string, bool> isMatch = (input, pattern) => input.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0;
            return isMatch(shortcut.Name, query ?? string.Empty) || isMatch(shortcut.Location, query ?? string.Empty);
        }

        private IEnumerable<Shortcut> ReadShortcuts(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                foreach (var line in _fileSystem.FileReadLines(path))
                {
                    var shortcut = ReadShortcut(line);
                    if (shortcut != null)
                        yield return shortcut;
                }
            }
        }

        private void WriteShortcuts(string path, IEnumerable<Shortcut> shortcuts, bool append)
        {
            if (!string.IsNullOrWhiteSpace(path) && shortcuts != null)
            {
                _fileSystem.FileWriteLines(path, shortcuts.Select(WriteShortcut).ToArray(), append);
            }
        }

        private Shortcut ReadShortcut(string line)
        {
            Shortcut shortcut = null;

            var parts = line?.Split('\t');
            if (parts?.Length == 3 && parts?.All(p => !string.IsNullOrWhiteSpace(p)) == true)
                shortcut = new Shortcut(parts[0], parts[1], parts[2]);

            return shortcut;
        }

        private string WriteShortcut(Shortcut shortcut)
        {
            return shortcut != null ? $"{shortcut.Name}\t{shortcut.Location}\t{shortcut.Target}" : null;
        }
    }
}
