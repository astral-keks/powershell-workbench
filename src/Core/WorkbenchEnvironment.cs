using AstralKeks.Workbench.Common.Resources;
using AstralKeks.Workbench.Core.Management;

namespace AstralKeks.Workbench.Core
{
    public class WorkbenchEnvironment
    {
        private readonly ResourceManager _resourceManager;
        private readonly ConfigurationManager _configurationManager;
        private readonly MacrosManager _macrosManager;
        private readonly UserspaceManager _userspaceManager;
        private readonly WorkspaceManager _workspaceManager;
        private readonly ApplicationManager _applicationManager;
        private readonly InstallationManager _installationManager;
        private readonly ToolkitManager _toolkitManager;

        public WorkbenchEnvironment()
        {
            _resourceManager = new ResourceManager(typeof(WorkbenchEnvironment));
            _configurationManager = new ConfigurationManager(_resourceManager);
            _userspaceManager = new UserspaceManager();
            _workspaceManager = new WorkspaceManager(_userspaceManager, _configurationManager, _resourceManager);
            _macrosManager = new MacrosManager(_workspaceManager, _userspaceManager);
            _applicationManager = new ApplicationManager(_workspaceManager, _userspaceManager, _configurationManager, _macrosManager);
            _installationManager = new InstallationManager(_userspaceManager, _configurationManager);
            _toolkitManager = new ToolkitManager(_workspaceManager, _userspaceManager, _configurationManager, _resourceManager, _macrosManager);
        }

        public InstallationManager InstallationManager => _installationManager;

        public ConfigurationManager ConfigurationManager => _configurationManager;
        
        public ApplicationManager ApplicationManager => _applicationManager;

        public UserspaceManager UserspaceManager => _userspaceManager;

        public WorkspaceManager WorkspaceManager => _workspaceManager;

        public ToolkitManager ToolkitManager => _toolkitManager;
    }
}
