using System;

namespace AstralKeks.Workbench.Core.Data
{
    public class Context
    {
        private readonly string _workspaceDirectory;
        private readonly string _userspaceDirectory;

        public Context(string workspaceDirectory, string userspaceDirectory)
        {
            _workspaceDirectory = workspaceDirectory ?? throw new ArgumentNullException(nameof(workspaceDirectory));
            _userspaceDirectory = userspaceDirectory ?? throw new ArgumentNullException(nameof(userspaceDirectory));
        }

        public string WorkspaceDirectory => _workspaceDirectory;

        public string UserspaceDirectory => _userspaceDirectory;
    }
}
