using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Resources;
using System;

namespace AstralKeks.Workbench.Repositories
{
    public class ProfileRepository
    {
        private readonly GlobalContext _globalContext;
        private readonly UserspaceContext _userspaceContext;
        private readonly WorkspaceContext _workspaceContext;

        public ProfileRepository(GlobalContext globalContext, UserspaceContext userspaceContext, WorkspaceContext workspaceContext)
        {
            _globalContext = globalContext ?? throw new ArgumentNullException(nameof(globalContext));
            _userspaceContext = userspaceContext ?? throw new ArgumentNullException(nameof(userspaceContext));
            _workspaceContext = workspaceContext ?? throw new ArgumentNullException(nameof(workspaceContext));
        }

        public string AllUserspaces()
        {
            return PathBuilder.Complete(_globalContext.UserDirectory, Files.AllUserspacesPs1);
        }

        public string AllWorkspaces()
        {
            return PathBuilder.Complete(_globalContext.UserDirectory, Files.AllWorkspacesPs1);
        }

        public string Workspaces(string userspaceDirectory = null)
        {
            userspaceDirectory = userspaceDirectory ?? _userspaceContext.CurrentUserspaceDirectory;
            return PathBuilder.Complete(userspaceDirectory, Files.WorkspacesPs1);
        }

        public string CurrentUserspace(string userspaceDirectory = null)
        {
            userspaceDirectory = userspaceDirectory ?? _userspaceContext.CurrentUserspaceDirectory;
            return PathBuilder.Complete(userspaceDirectory, Files.UserspacePs1);
        }

        public string CurrentWorkspace(string workspaceDirectory = null)
        {
            workspaceDirectory = workspaceDirectory ?? _workspaceContext.CurrentWorkspaceDirectory;
            return PathBuilder.Complete(workspaceDirectory, Files.WorkspacePs1);
        }
    }
}
