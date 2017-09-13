using System;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Repositories;
using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Context;

namespace AstralKeks.Workbench.Controllers
{
    public class WorkspaceController
    {
        private readonly WorkspaceContext _workspaceContext;
        private readonly WorkspaceRepository _workspaceRepository;
        private readonly FileSystem _fileSystem;

        public WorkspaceController(WorkspaceContext workspaceContext, WorkspaceRepository workspaceRepository, FileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _workspaceContext = workspaceContext ?? throw new ArgumentNullException(nameof(workspaceContext));
            _workspaceRepository = workspaceRepository ?? throw new ArgumentNullException(nameof(workspaceRepository));
        }
        
        public Workspace EnterWorkspace(string directory, Func<Workspace> onMissing, Func<Workspace> onFallback)
        {
            Workspace workspace;
            if (!string.IsNullOrWhiteSpace(directory))
            {
                workspace = _workspaceRepository.FindWorkspace(directory);
                if (workspace == null)
                    workspace = onMissing();
                if (workspace == null)
                    workspace = onFallback();
            }
            else
                workspace = _workspaceRepository.FindWorkspace(_workspaceContext.RecentWorkspaceDirectory);
            
            return EnterWorkspace(workspace);
        }

        public Workspace EnterWorkspace(Workspace workspace)
        {
            if (workspace != null)
            {
                _workspaceContext.CurrentWorkspaceDirectory = workspace.Directory;
                _workspaceContext.RecentWorkspaceDirectory = workspace.Directory;
                _fileSystem.SetCurrentDirectory(_workspaceContext.CurrentWorkspaceDirectory);
            }

            return workspace;
        }

        public Workspace ExitWorkspace()
        {
            var workspace = _workspaceRepository.GetWorkspace(_workspaceContext.CurrentWorkspaceDirectory);
            _workspaceContext.CurrentWorkspaceDirectory = null;

            return workspace;
        }
    }
}

