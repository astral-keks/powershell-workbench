using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using System;

namespace AstralKeks.Workbench.Common.Context
{
    public class GlobalContext
    {
        private const string _workbench = ".Workbench";

        private readonly SystemVariable _systemVariable;

        public GlobalContext(SystemVariable systemVariable)
        {
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
        }

        public string ApplicationDirectoryRoot
        {
            get => !string.IsNullOrWhiteSpace(_systemVariable.LocalAppData)
                ? _systemVariable.LocalAppData
                : _systemVariable.Home;
        }

        public string ApplicationDirectory
        {
            get => PathBuilder.Combine(ApplicationDirectoryRoot, _workbench); 
        }
    }
}
