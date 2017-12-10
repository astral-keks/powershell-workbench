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
                _workspaceContext.CurrentWorkspaceDirectory = workspace.Directory;
                _fileSystem.DirectorySetCurrent(_workspaceContext.CurrentWorkspaceDirectory);
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

