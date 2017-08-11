
using System.Collections.Generic;

namespace AstralKeks.Workbench.Core.Data
{
    public class Command
    {
        public const string Default = "Default";
        public const string Workspace = "Workspace";

        public string Name { get; set; }
        public List<string> Arguments { get; set; }

        public string[] DeleteVariables { get; set; }
        public bool RereadCurrentVariables { get; set; }
        public bool RereadMachineVariables { get; set; }
        public bool RereadUserVariables { get; set; }
        public bool ResetVariables { get; set; }
        public bool UseShellExecute { get; set; }
        public bool WaitForExit { get; set; }
        public bool NoWindow { get; set; }
        #region Unsupported
        //public bool RunAs { get; set; }
        #endregion
    }
}
