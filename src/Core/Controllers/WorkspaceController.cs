using System;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Repositories;
using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Context;

namespace AstralKeks.Workbench.Controllers
{
    public class WorkspaceController
    {
        private readonly SessionContext _sessionContext;
        private readonly WorkspaceRepository _workspaceRepository;
        private readonly FileSystem _fileSystem;

        public WorkspaceController(SessionContext sessionContext, WorkspaceRepository workspaceRepository, FileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
            _workspaceRepository = workspaceRepository ?? throw new ArgumentNullException(nameof(workspaceRepository));
        }

        public bool CheckWorkspace(string directory, Func<string, bool> shouldCreate)
        {
            var workspace = GetWorkspace(directory);
            if (workspace == null && shouldCreate(directory))
                workspace = _workspaceRepository.CreateWorkspace(directory);

            return workspace != null;
        }

        public Workspace UseWorkspace(string directory)
        {
            var workspace = GetWorkspace(directory);
            if (workspace != null)
            {
                _sessionContext.CurrentWorkspaceDirectory = workspace.Directory;
                _fileSystem.DirectorySetCurrent(_sessionContext.CurrentWorkspaceDirectory);
            }

            return workspace;
        }

        private Workspace GetWorkspace(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                directory = _fileSystem.DirectoryGetCurrent();

            return _workspaceRepository.FindWorkspace(directory);
        }
    }
}

