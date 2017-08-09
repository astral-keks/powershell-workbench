using AstralKeks.Workbench.Core.Data;
using System;

namespace AstralKeks.Workbench.Core.Management
{
    public class AliasManager
    {
        private readonly WorkspaceManager _workspaceManager;
        private readonly UserspaceManager _userspaceManager;
        private readonly ConfigurationManager _configurationManager;

        public AliasManager(WorkspaceManager workspaceManager, UserspaceManager userspaceManager,
            ConfigurationManager configurationManager)
        {
            _workspaceManager = workspaceManager ?? throw new ArgumentNullException(nameof(workspaceManager));
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
        }

        public Alias[] GetAliases()
        {
            var workspaceDirectory = _workspaceManager.GetWorkspaceDirectory();
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            return _configurationManager.GetAliasConfig(workspaceDirectory, userspaceDirectory);
        }
    }
}
