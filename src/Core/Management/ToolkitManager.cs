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

        public Repository[] GetToolkitRepositories()
        {
            var workspaceDirectory = _workspaceManager.GetWorkspaceDirectory();
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            var repositories = _configurationManager.GetRepositoryConfig(workspaceDirectory, userspaceDirectory);
            foreach (var repository in repositories)
                repository.Directory = _macrosManager.ResolveMacros(repository.Directory);
            return repositories;
        }

        public string ResolveToolkitModule(string moduleName, string moduleBase, ICollection<Repository> repositories)
        {
            Repository repository = null;
            if (!string.IsNullOrWhiteSpace(moduleName) && !string.IsNullOrWhiteSpace(moduleBase))
            {
                repository = repositories
                    .Where(r => !string.IsNullOrWhiteSpace(r.Directory))
                    .FirstOrDefault(r => moduleBase.StartsWith(r.Directory));
            }
            if (repository != null && repository.Modules != null && repository.Modules.Any())
            {
                if (repository.Modules.All(m => m != moduleName))
                    repository = null;
            }

            return repository != null ? moduleName : null;
        }
    }
}
