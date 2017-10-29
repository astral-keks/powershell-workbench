using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Template;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Configuration;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Repositories;
using AstralKeks.Workbench.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AstralKeks.Workbench.Controllers
{
    public class ShortcutController
    {
        private readonly FileSystem _fileSystem;
        private readonly WorkspaceContext _workspaceContext;
        private readonly UserspaceContext _userspaceContext;
        private readonly TemplateProcessor _templateProcessor;
        private readonly ResourceRepository _resourceRepository;
        private readonly ShortcutRepository _shortcutRepository;

        public ShortcutController(FileSystem fileSystem, WorkspaceContext workspaceContext, UserspaceContext userspaceContext,
            TemplateProcessor templateProcessor, ResourceRepository resourceRepository, ShortcutRepository shortcutRepository)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _workspaceContext = workspaceContext ?? throw new ArgumentNullException(nameof(workspaceContext));
            _userspaceContext = userspaceContext ?? throw new ArgumentNullException(nameof(userspaceContext));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
            _shortcutRepository = shortcutRepository ?? throw new ArgumentNullException(nameof(shortcutRepository));
        }

        public IEnumerable<Shortcut> FindShortcut(string query = null)
        {
            var workspaceConfiguration = GetDiscoveryConfiguration(_workspaceContext.CurrentWorkspaceDirectory);
            var userspaceConfiguration = GetDiscoveryConfiguration(_userspaceContext.CurrentUserspaceDirectory);

            var shorcuts = _shortcutRepository.FindShortcut(query);
            var workspaceShortcuts = DiscoverShortcuts(workspaceConfiguration.Dynamic, false).Where(s => s.IsMatch(query));
            var userspaceShortcuts = DiscoverShortcuts(userspaceConfiguration.Dynamic, false).Where(s => s.IsMatch(query));

            return shorcuts.Concat(workspaceShortcuts).Concat(userspaceShortcuts);
        }

        public void SynchronizeShortcuts(bool inWorkspace, bool inUserspace)
        {
            if (inWorkspace)
            {
                var configuration = GetDiscoveryConfiguration(_workspaceContext.CurrentWorkspaceDirectory);
                var shortcuts = DiscoverShortcuts(configuration.Synchronized, true);
                _shortcutRepository.ClearWorkspaceShortcuts();
                _shortcutRepository.AddWorkspaceShortcuts(shortcuts);
            }
            if (inUserspace)
            {
                var configuration = GetDiscoveryConfiguration(_userspaceContext.CurrentUserspaceDirectory);
                var shortcuts = DiscoverShortcuts(configuration.Synchronized, true);
                _shortcutRepository.ClearUserspaceShortcuts();
                _shortcutRepository.AddUserspaceShortcuts(shortcuts);
            }
        }

        private ShortcutConfig GetDiscoveryConfiguration(string directory)
        {
            var resourcePath = PathBuilder.Complete(directory, Directories.Config, Directories.Workbench, Files.DiscoveryJson);
            var resource = _templateProcessor.Transform(_resourceRepository.GetResource(resourcePath));
            return resource?.Read<ShortcutConfig>() ?? new ShortcutConfig();
        }

        private IEnumerable<Shortcut> DiscoverShortcuts(List<ShortcutPattern> patterns, bool recursively)
        {
            foreach (var patternGroup in patterns.GroupBy(p => p.RootDirectory))
            {
                var rootDirectory = patternGroup.Key;
                var patternGroupList = patternGroup.ToList();
                foreach (var shortcut in DiscoverShortcuts(rootDirectory, patternGroupList, recursively))
                    yield return shortcut;
            }
        }

        private IEnumerable<Shortcut> DiscoverShortcuts(string directory, List<ShortcutPattern> patterns, bool recursively)
        {
            if (!string.IsNullOrWhiteSpace(directory))
            {
                foreach (var path in _fileSystem.DirectoryList(directory))
                {
                    var shortcut = patterns.Select(p => DiscoverShortcut(path, p)).FirstOrDefault();
                    if (shortcut != null)
                    {
                        yield return shortcut;
                    }
                    else if (_fileSystem.DirectoryExists(path) && recursively)
                    {
                        foreach (var childShortcut in DiscoverShortcuts(path, patterns, true))
                            yield return childShortcut;
                    }
                } 
            }
        }

        private Shortcut DiscoverShortcut(string path, ShortcutPattern pattern)
        {
            Shortcut shortcut = null;

            if (!string.IsNullOrEmpty(pattern.PathPattern))
            {
                var match = Regex.Match(path, pattern.PathPattern.Replace("\\", "\\\\"));
                if (match.Success && pattern.InnerPathPatterns.Any() && _fileSystem.DirectoryExists(path))
                {
                    foreach (var innerPath in _fileSystem.DirectoryList(path))
                    {
                        if (!pattern.InnerPathPatterns.All(p => Regex.IsMatch(innerPath, p)))
                            match = null;
                    }
                }

                if (match?.Success == true)
                {
                    var name = Path.GetFileName(path);
                    var location = pattern.Location;
                    if (string.IsNullOrWhiteSpace(location))
                        location = match.Groups["location"].Value;
                    if (string.IsNullOrWhiteSpace(location))
                        location = Path.GetFileName(pattern.RootDirectory);
                    shortcut = new Shortcut(name, location, path);
                }
            }
            
            return shortcut;
        }
    }
}
