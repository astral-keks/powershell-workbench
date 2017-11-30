using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using System;

namespace AstralKeks.Workbench.Common.Context
{
    public class UserspaceContext
    {
        private const string _default = "Default";
        private const string _workbench = ".Workbench";

        private readonly SystemVariable _systemVariable;

        public UserspaceContext(SystemVariable systemVariable)
        {
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
        }
        
        public string UserspaceRootDirectory
        {
            get { return PathBuilder.Combine(_systemVariable.UserDirectory, _workbench); }
        }

        public string DefaultUserspaceDirectory
        {
            get { return PathBuilder.Combine(UserspaceRootDirectory, _default); }
        }

        public string CurrentUserspaceDirectory
        {
            get { return _systemVariable.UserspaceDirectory; }
            set { _systemVariable.UserspaceDirectory = value; }
        }
    }
}
