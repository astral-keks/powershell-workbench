using AstralKeks.Workbench.Core.Management;

namespace AstralKeks.Workbench.Core
{
    public class WorkbenchEnvironment
    {
        private readonly FileSystemManager _fileSystemManager;
        private readonly MacrosManager _macrosManager;
        private readonly ResourceManager _resourceManager;
        private readonly ConfigurationManager _configurationManager;
        private readonly WorkspaceManager _workspaceManager;
        private readonly ApplicationManager _applicationManager;
        private readonly ToolkitManager _toolkitManager;

        public WorkbenchEnvironment()
        {
            _fileSystemManager = new FileSystemManager();
            _macrosManager = new MacrosManager(_fileSystemManager);
            _resourceManager = new ResourceManager(_fileSystemManager);
            _configurationManager = new ConfigurationManager(_resourceManager);
            _workspaceManager = new WorkspaceManager(_configurationManager, _fileSystemManager, _resourceManager);
            _applicationManager = new ApplicationManager(_configurationManager, _workspaceManager, _macrosManager);
            _toolkitManager = new ToolkitManager(_configurationManager, _workspaceManager, _macrosManager);
        }

        public ApplicationManager ApplicationManager => _applicationManager;

        public WorkspaceManager WorkspaceManager => _workspaceManager;

        public ToolkitManager ToolkitManager => _toolkitManager;

        public FileSystemManager FileSystemManager => _fileSystemManager;
    }
}
