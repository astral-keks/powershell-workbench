using System;
using System.IO;

namespace AstralKeks.Workbench.Core.Data
{
    public class Context
    {
        private readonly string _workspaceDirectory;

        public Context(string workspaceDirectory)
        {
            if (workspaceDirectory == null)
                throw new ArgumentNullException(nameof(workspaceDirectory));

            _workspaceDirectory = workspaceDirectory;
        }

        public Context ApplyToEnvironment()
        {
            Directory.SetCurrentDirectory(_workspaceDirectory);
            return this;
        }

        public Context ApplyToCmdlet(Action<string> scriptInvoker)
        {
            scriptInvoker($"Set-Location '{_workspaceDirectory}'");
            return this;
        }
    }
}
