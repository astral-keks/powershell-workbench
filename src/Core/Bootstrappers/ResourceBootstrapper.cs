using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Repositories;
using Autofac;
using System;

namespace AstralKeks.Workbench.Bootstrappers
{
    public class ResourceBootstrapper : IStartable
    {
        private readonly SessionContext _sessionContext;
        private readonly UserspaceRepository _userspaceRepository;
        private readonly WorkspaceRepository _workspaceRepository;

        public ResourceBootstrapper(SessionContext sessionContext,
            UserspaceRepository userspaceRepository, WorkspaceRepository workspaceRepository)
        {
            _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
            _userspaceRepository = userspaceRepository ?? throw new ArgumentNullException(nameof(userspaceRepository));
            _workspaceRepository = workspaceRepository ?? throw new ArgumentNullException(nameof(workspaceRepository));
        }

        public void Start()
        {
            var defaultUserspaceDirectory = _sessionContext.DefaultUserspaceDirectory;
            var currentUserspaceDirectory = _sessionContext.CurrentUserspaceDirectory;
            var currentWorkspaceDirectory = _sessionContext.CurrentWorkspaceDirectory;

            BootstrapUserspace(defaultUserspaceDirectory);
            if (!string.Equals(currentUserspaceDirectory, defaultUserspaceDirectory, StringComparison.OrdinalIgnoreCase))
                BootstrapUserspace(currentUserspaceDirectory);
            if (!string.IsNullOrWhiteSpace(currentWorkspaceDirectory))
                BootstrapWorkspace(currentWorkspaceDirectory);
        }
        
        protected virtual void BootstrapUserspace(string userspaceDirectory)
        {
            if (!string.IsNullOrWhiteSpace(userspaceDirectory))
            {
                var userspace = _userspaceRepository.DefineUserspace(userspaceDirectory);
                _userspaceRepository.CreateUserspace(userspace);
            }
        }

        protected virtual void BootstrapWorkspace(string workspaceDirectory)
        {
            if (!string.IsNullOrWhiteSpace(workspaceDirectory))
            {
                var workspace = _workspaceRepository.DefineWorkspace(workspaceDirectory);
                _workspaceRepository.CreateWorkspace(workspace); 
            }
        }
    }
}
