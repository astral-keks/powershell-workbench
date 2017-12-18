using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using System;

namespace AstralKeks.Workbench.Common.Context
{
    public class SessionContext
    {
        private const string _default = "Default";

        private readonly GlobalContext _globalContext;
        private readonly SystemVariable _systemVariable;

        public SessionContext(SystemVariable systemVariable, GlobalContext globalContext)
        {
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
            _globalContext = globalContext ?? throw new ArgumentNullException(nameof(globalContext));
        }

        public string DefaultUserspaceDirectory
        {
            get => PathBuilder.Combine(_globalContext.ApplicationDirectory, _default);
        }

        public string CurrentUserspaceDirectory
        {
            get => _systemVariable.UserspaceDirectory ?? DefaultUserspaceDirectory;
            set => _systemVariable.UserspaceDirectory = value;
        }

        public string CurrentWorkspaceDirectory
        {
            get => _systemVariable.WorkspaceDirectory;
            set => _systemVariable.WorkspaceDirectory = value;
        }
    }
}
