using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Resources;
using System;

namespace AstralKeks.Workbench.Repositories
{
    public class ProfileRepository
    {
        private readonly GlobalContext _globalContext;
        private readonly SessionContext _sessionContext;

        public ProfileRepository(GlobalContext globalContext, SessionContext sessionContext)
        {
            _globalContext = globalContext ?? throw new ArgumentNullException(nameof(globalContext));
            _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
        }

        public string AllUserspaces()
        {
            return PathBuilder.Complete(_globalContext.UserspacesDirectory, Files.AllUserspacesPs1);
        }

        public string AllWorkspaces()
        {
            return PathBuilder.Complete(_globalContext.UserspacesDirectory, Files.AllWorkspacesPs1);
        }

        public string Workspaces(string userspaceDirectory = null)
        {
            userspaceDirectory = userspaceDirectory ?? _sessionContext.CurrentUserspaceDirectory;
            return PathBuilder.Complete(userspaceDirectory, Files.WorkspacesPs1);
        }

        public string CurrentUserspace(string userspaceDirectory = null)
        {
            userspaceDirectory = userspaceDirectory ?? _sessionContext.CurrentUserspaceDirectory;
            return PathBuilder.Complete(userspaceDirectory, Files.UserspacePs1);
        }

        public string CurrentWorkspace(string workspaceDirectory = null)
        {
            workspaceDirectory = workspaceDirectory ?? _sessionContext.CurrentWorkspaceDirectory;
            return PathBuilder.Complete(workspaceDirectory, Files.WorkspacePs1);
        }
    }
}
