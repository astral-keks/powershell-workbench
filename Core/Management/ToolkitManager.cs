using System.Collections.Generic;
using System;
using System.Linq;

namespace AstralKeks.Workbench.Core.Management
{
    public class ToolkitManager
    {
        private readonly ConfigurationManager _configurationManager;
        private readonly WorkspaceManager _workspaceManager;
        private readonly MacrosManager _macrosManager;

        public ToolkitManager(ConfigurationManager configurationManager, WorkspaceManager workspaceManager,
            MacrosManager macrosManager)
        {
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
            _workspaceManager = workspaceManager ?? throw new ArgumentNullException(nameof(workspaceManager));
            _macrosManager = macrosManager ?? throw new ArgumentNullException(nameof(macrosManager));
        }

        public IEnumerable<string> GetToolkitDirectories()
        {
            var workspaceDirectory = _workspaceManager.GetCurrentWorkspaceDirectory();
            var repositoryConfig = _configurationManager.GetRepositoryConfig(workspaceDirectory);
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
