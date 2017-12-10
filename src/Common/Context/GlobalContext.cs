using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstralKeks.Workbench.Common.Context
{
    public class GlobalContext
    {
        private const string _workbench = ".Workbench";
        private const string _powerShellTools = ".PowerShellTools";

        private readonly SystemVariable _systemVariable;

        public GlobalContext(SystemVariable systemVariable)
        {
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
        }

        public string AltUserDirectory
        {
            get { return PathBuilder.Combine(_systemVariable.UserDirectory, _powerShellTools); }
        }

        public string UserDirectory
        {
            get { return PathBuilder.Combine(_systemVariable.UserDirectory, _workbench); }
        }

    }
}
