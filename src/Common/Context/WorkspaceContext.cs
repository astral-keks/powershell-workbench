using AstralKeks.Workbench.Common.Infrastructure;
using System;

namespace AstralKeks.Workbench.Common.Context
{
    public class WorkspaceContext
    {
        private readonly FileSystem _fileSystem;
        private readonly SystemVariable _systemVariable;

        public WorkspaceContext(FileSystem fileSystem, SystemVariable systemVariable)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
        }

        public string CurrentWorkspaceDirectory
        {
            get { return _systemVariable.WorkspaceDirectory; }
            set { _systemVariable.WorkspaceDirectory = value; }
        }

        public string RecentWorkspaceDirectory
        {
            get { return _systemVariable.RecentWorkspaceDirectory; }
            set { _systemVariable.RecentWorkspaceDirectory = value; }
        }
    }
}
