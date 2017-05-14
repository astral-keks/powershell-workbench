using System.Collections.Generic;
using System;
using System.Linq;
using AstralKeks.Workbench.Core.Data;

namespace AstralKeks.Workbench.Core.Management
{
    public class ToolkitManager
    {
        private readonly WorkspaceManager _workspaceManager;
        private readonly UserspaceManager _userspaceManager;
        private readonly ConfigurationManager _configurationManager;
        private readonly MacrosManager _macrosManager;

        public ToolkitManager(WorkspaceManager workspaceManager, UserspaceManager userspaceManager,
            ConfigurationManager configurationManager, MacrosManager macrosManager)
        {
            _workspaceManager = workspaceManager ?? throw new ArgumentNullException(nameof(workspaceManager));
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
            _macrosManager = macrosManager ?? throw new ArgumentNullException(nameof(macrosManager));
        }

        public string[] GetToolkitDirectories()
        {
            var workspaceDirectory = _workspaceManager.GetWorkspaceDirectory();
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            var repositoryConfig = _configurationManager.GetRepositoryConfig(workspaceDirectory, userspaceDirectory);
            return repositoryConfig.Select(r => _macrosManager.ResolveMacros(r.Directory)).ToArray();
        }

        public Toolkit ResolveToolkit(string moduleName, string moduleBase, ICollection<string> directories)
        {
            string directory = null;
            if (!string.IsNullOrWhiteSpace(moduleName) && !string.IsNullOrWhiteSpace(moduleBase))
                directory = directories.FirstOrDefault(moduleBase.StartsWith);

            return directory != null ? new Toolkit(directory, moduleName) : null;
        }

    }
}
