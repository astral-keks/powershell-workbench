using System;

namespace AstralKeks.Workbench.Core.Data
{
    public class Context
    {
        public Context(string workspaceDirectory, string userspaceDirectory)
        {
            WorkspaceDirectory = workspaceDirectory ?? throw new ArgumentNullException(nameof(workspaceDirectory));
            UserspaceDirectory = userspaceDirectory ?? throw new ArgumentNullException(nameof(userspaceDirectory));
        }

        public string WorkspaceDirectory { get; }

        public string UserspaceDirectory { get; }
    }
}
