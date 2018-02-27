using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using System;

namespace AstralKeks.Workbench.Common.Context
{
    public class GlobalContext
    {
        private const string _workbench = ".Workbench";
        private const string _userspaces = "Userspaces";

        private readonly SystemVariable _systemVariable;

        public GlobalContext(SystemVariable systemVariable)
        {
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
        }

        public string UserDirectory
        {
            get => !string.IsNullOrWhiteSpace(_systemVariable.LocalAppData)
                ? _systemVariable.LocalAppData
                : _systemVariable.Home;
        }

        public string WorkbenchDirectory
        {
            get => PathBuilder.Combine(UserDirectory, _workbench); 
        }

        public string UserspacesDirectory
        {
            get => PathBuilder.Combine(WorkbenchDirectory, _userspaces);
        }
    }
}
