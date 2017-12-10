using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using System;

namespace AstralKeks.Workbench.Common.Context
{
    public class UserspaceContext
    {
        private const string _default = "Default";

        private readonly GlobalContext _globalContext;
        private readonly SystemVariable _systemVariable;

        public UserspaceContext(GlobalContext globalContext, SystemVariable systemVariable)
        {
            _globalContext = globalContext ?? throw new ArgumentNullException(nameof(globalContext));
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
        }

        public string DefaultUserspaceDirectory
        {
            get { return PathBuilder.Combine(_globalContext.UserDirectory, _default); }
        }

        public string CurrentUserspaceDirectory
        {
            get { return _systemVariable.UserspaceDirectory; }
            set { _systemVariable.UserspaceDirectory = value; }
        }
    }
}
