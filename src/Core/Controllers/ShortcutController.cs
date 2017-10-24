using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Infrastructure;
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
        private readonly ResourceRepository _resourceRepository;
        private readonly ShortcutRepository _shortcutRepository;

        public ShortcutController(FileSystem fileSystem, WorkspaceContext workspaceContext, UserspaceContext userspaceContext,
            ResourceRepository resourceRepository, ShortcutRepository shortcutRepository)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _workspaceContext = workspaceContext ?? throw new ArgumentNullException(nameof(workspaceContext));
            _userspaceContext = userspaceContext ?? throw new ArgumentNullException(nameof(userspaceContext));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
            _shortcutRepository = shortcutRepository ?? throw new ArgumentNullException(nameof(shortcutRepository));
        }

        public void SynchronizeShortcuts(bool inWorkspace, bool inUserspace)
        {
            var workspaceShortcuts = inWorkspace
                ? SynchronizeShortcuts(_workspaceContext.CurrentWorkspaceDirectory).ToList()
                : null;
            var userspaceShortcuts = inUserspace
                ? SynchronizeShortcuts(_userspaceContext.CurrentUserspaceDirectory).ToList()
                : null;

            _shortcutRepository.ClearShortcuts();
            _shortcutRepository.AddShortcuts(workspaceShortcuts, userspaceShortcuts);
        }

        private IEnumerable<Shortcut> SynchronizeShortcuts(string directory)
        {
            var configuration = GetConfiguration(directory);
            if (configuration == null)
                throw new InvalidOperationException("Unable to get shortcut configuration");

            foreach (var patternGroup in configuration.Dynamic.GroupBy(p => p.RootDirectory))
            {
                var rootDirectory = patternGroup.Key;
                var patterns = patternGroup.ToList();
                foreach (var shortcut in DiscoverShortcuts(rootDirectory, patterns))
                    yield return shortcut;
            }
        }

        private ShortcutConfig GetConfiguration(string directory)
        {
            var resourcePath = PathBuilder.Complete(directory, Directories.Config, Files.WBShortcutJson);
            var resource = _resourceRepository.GetResource(resourcePath);
            return resource?.Read<ShortcutConfig>();
        }

        private IEnumerable<Shortcut> DiscoverShortcuts(string directory, List<ShortcutPattern> patterns)
        {
            if (!string.IsNullOrWhiteSpace(directory))
            {
                foreach (var path in _fileSystem.DirectoryList(directory))
                {
                    var shortcut = patterns.Select(p => ParseShortcut(path, p)).FirstOrDefault();
                    if (shortcut != null)
                    {
                        yield return shortcut;
                    }
                    else if (_fileSystem.DirectoryExists(path))
                    {
                        foreach (var childShortcut in DiscoverShortcuts(path, patterns))
                            yield return childShortcut;
                    }
                } 
            }
        }

        private Shortcut ParseShortcut(string path, ShortcutPattern pattern)
        {
            Shortcut shortcut = null;

            if (!string.IsNullOrEmpty(pattern.PathPattern))
            {
                var match = Regex.Match(path, pattern.PathPattern);
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
