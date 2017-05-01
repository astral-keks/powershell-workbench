
namespace AstralKeks.Workbench.Core.Data
{
    public class Command
    {
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
