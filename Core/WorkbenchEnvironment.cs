using AstralKeks.Workbench.Core.Management;

namespace AstralKeks.Workbench.Core
{
    public class WorkbenchEnvironment
    {
        private readonly FileSystemManager _fileSystemManager;
        private readonly InstallationManager _installationManager;
        private readonly MacrosManager _macrosManager;
        private readonly ResourceManager _resourceManager;
        private readonly ConfigurationManager _configurationManager;
        private readonly UserspaceManager _userspaceManager;
        private readonly WorkspaceManager _workspaceManager;
        private readonly ApplicationManager _applicationManager;
        private readonly ToolkitManager _toolkitManager;
        private readonly ContextManager _contextManager;

        public WorkbenchEnvironment()
        {
            _fileSystemManager = new FileSystemManager();
            _installationManager = new InstallationManager(_fileSystemManager);
            _macrosManager = new MacrosManager(_fileSystemManager);
            _resourceManager = new ResourceManager(_fileSystemManager);
            _configurationManager = new ConfigurationManager(_resourceManager);
            _userspaceManager = new UserspaceManager(_fileSystemManager);
            _workspaceManager = new WorkspaceManager(_userspaceManager, _configurationManager, _fileSystemManager, _resourceManager);
            _applicationManager = new ApplicationManager(_workspaceManager, _userspaceManager, _configurationManager, _macrosManager);
            _toolkitManager = new ToolkitManager(_workspaceManager, _userspaceManager, _configurationManager, _macrosManager);
            _contextManager = new ContextManager(_workspaceManager, _userspaceManager);
        }

        public InstallationManager InstallationManager => _installationManager;

        public ApplicationManager ApplicationManager => _applicationManager;

        public UserspaceManager UserspaceManager => _userspaceManager;

        public WorkspaceManager WorkspaceManager => _workspaceManager;

        public ToolkitManager ToolkitManager => _toolkitManager;

        public FileSystemManager FileSystemManager => _fileSystemManager;

        public ContextManager ContextManager => _contextManager;
    }
}
