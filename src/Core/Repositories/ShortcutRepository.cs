using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Template;
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
        private readonly SharedContext _sharedContext;
        private readonly TemplateProcessor _templateProcessor;
        private readonly ResourceRepository _resourceRepository;

        public ShortcutRepository(SharedContext sharedContext, TemplateProcessor templateProcessor, ResourceRepository resourceRepository)
        {
            _sharedContext = sharedContext ?? throw new ArgumentNullException(nameof(sharedContext));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public void AddWorkspaceShortcuts(IEnumerable<Shortcut> shortcuts = null)
        {
            var shortcutWorkspaceStoragePath = PathBuilder.Complete(_sharedContext.CurrentWorkspaceDirectory,
                Directories.Temp, Directories.Workbench, Files.ShortcutJson);
            
            WriteShortcuts(shortcutWorkspaceStoragePath, shortcuts, true);
        }

        public void AddUserspaceShortcuts(IEnumerable<Shortcut> shortcuts = null)
        {
            var shortcutUserspaceStoragePath = PathBuilder.Complete(_sharedContext.CurrentUserspaceDirectory,
                Directories.Temp, Directories.Workbench, Files.ShortcutJson);

            WriteShortcuts(shortcutUserspaceStoragePath, shortcuts, true);
        }

        public void ClearWorkspaceShortcuts()
        {
            var shortcutWorkspaceStoragePath = PathBuilder.Complete(_sharedContext.CurrentWorkspaceDirectory,
                Directories.Temp, Directories.Workbench, Files.ShortcutJson);

            WriteShortcuts(shortcutWorkspaceStoragePath, Enumerable.Empty<Shortcut>(), false);
        }

        public void ClearUserspaceShortcuts()
        {
            var shortcutUserspaceStoragePath = PathBuilder.Complete(_sharedContext.CurrentUserspaceDirectory,
                Directories.Temp, Directories.Workbench, Files.ShortcutJson);

            WriteShortcuts(shortcutUserspaceStoragePath, Enumerable.Empty<Shortcut>(), false);
        }

        public IEnumerable<Shortcut> FindShortcut(string query = null)
        {
            var paths = new[] 
            {
                PathBuilder.Complete(_sharedContext.CurrentWorkspaceDirectory,
                    Directories.Temp, Directories.Workbench, Files.ShortcutJson),
                PathBuilder.Complete(_sharedContext.CurrentWorkspaceDirectory,
                    Directories.Config, Directories.Workbench, Files.ShortcutJson),
                PathBuilder.Complete(_sharedContext.CurrentUserspaceDirectory,
                    Directories.Temp, Directories.Workbench, Files.ShortcutJson),
                PathBuilder.Complete(_sharedContext.CurrentUserspaceDirectory,
                    Directories.Config, Directories.Workbench, Files.ShortcutJson)
            };

            var foundShortcuts = new HashSet<string>();
            foreach (var path in paths)
            {
                foreach (var shortcut in ReadShortcuts(path).Where(s => s.ToString().Like(query)))
                {
                    if (foundShortcuts.Add(shortcut.ToString()))
                        yield return shortcut;
                }
            }
        }

        public IEnumerable<Shortcut> ResolveShortcut(string query)
        {
            var paths = new[]
            {
                PathBuilder.Complete(_sharedContext.CurrentWorkspaceDirectory,
                    Directories.Temp, Directories.Workbench, Files.ShortcutJson),
                PathBuilder.Complete(_sharedContext.CurrentWorkspaceDirectory,
                    Directories.Config, Directories.Workbench, Files.ShortcutJson),
                PathBuilder.Complete(_sharedContext.CurrentUserspaceDirectory,
                    Directories.Temp, Directories.Workbench, Files.ShortcutJson),
                PathBuilder.Complete(_sharedContext.CurrentUserspaceDirectory,
                    Directories.Config, Directories.Workbench, Files.ShortcutJson)
            };

            var foundShortcuts = new HashSet<string>();
            foreach (var path in paths)
            {
                foreach (var shortcut in ReadShortcuts(path).Where(s => s.ToString().Matches(query)))
                {
                    if (foundShortcuts.Add(shortcut.ToString()))
                        yield return shortcut;
                }
            }
        }

        private IEnumerable<Shortcut> ReadShortcuts(string path)
        {
            var shortcutsResource =_resourceRepository.GetResource(path);
            shortcutsResource = _templateProcessor.Transform(shortcutsResource);
            if (shortcutsResource != null && shortcutsResource.CanRead)
            {
                var shortcuts = shortcutsResource.Read<List<Shortcut>>();
                if (shortcuts != null)
                {
                    foreach (var shortcut in shortcuts)
                        yield return shortcut;
                }
            }
        }

        private void WriteShortcuts(string path, IEnumerable<Shortcut> shortcuts, bool append)
        {
            var shortcutsResource = _resourceRepository.CreateResource(path);
            if (append && shortcutsResource.CanRead)
            {
                var existingShortcuts = shortcutsResource.Read<List<Shortcut>>();
                if (existingShortcuts != null)
                    shortcuts = existingShortcuts.Concat(shortcuts);
            }

            shortcuts = shortcuts.ToList();
            shortcutsResource.Write((List<Shortcut>)shortcuts);
        }
    }
}
