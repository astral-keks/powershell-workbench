using System.Collections.Generic;
using System;
using System.Linq;

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

        public IEnumerable<string> GetToolkitDirectories()
        {
            var workspaceDirectory = _workspaceManager.GetWorkspaceDirectory();
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            var repositoryConfig = _configurationManager.GetRepositoryConfig(workspaceDirectory, userspaceDirectory);
            return repositoryConfig.Select(r => _macrosManager.ResolveMacros(r.Directory));
        }

        public string ResolveToolkitModule(string moduleName, string moduleBase, ICollection<string> directories)
        {
            var isResolved = !string.IsNullOrWhiteSpace(moduleName) &&
                !string.IsNullOrWhiteSpace(moduleBase) &&
                directories.Any(moduleBase.StartsWith);

            return isResolved ? moduleName : null;
        }

    }
}
