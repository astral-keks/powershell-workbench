
namespace AstralKeks.Workbench.Core.Data
{
    public class Command
    {
        public const string Default = "Default";
        public const string Workspace = "Workspace";

        public string Name { get; set; }
        public string Arguments { get; set; }
        public bool UseShellExecute { get; set; }
        public bool WaitForExit { get; set; }
        public bool NoWindow { get; set; }
        #region Unsupported
        //public bool RunAs { get; set; }
        //public bool RereadEnvironment { get; set; } 
        #endregion
    }
}
