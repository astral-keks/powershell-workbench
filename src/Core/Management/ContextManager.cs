using AstralKeks.Workbench.Core.Data;
using System;

namespace AstralKeks.Workbench.Core.Management
{
    public class ContextManager
    {
        private readonly WorkspaceManager _workspaceManager;
        private readonly UserspaceManager _userspaceManager;

        public ContextManager(WorkspaceManager workspaceManager, UserspaceManager userspaceManager)
        {
            _workspaceManager = workspaceManager ?? throw new ArgumentNullException(nameof(workspaceManager));
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
        }

        public Context GetContext()
        {
            var workspaceDirectory = _workspaceManager.GetWorkspaceDirectory();
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            return new Context(workspaceDirectory, userspaceDirectory);
        }
    }
}
