using AstralKeks.Workbench.Common.Data;
using AstralKeks.Workbench.Common.FileSystem;
using AstralKeks.Workbench.Core.Resources;
using System.IO;

namespace AstralKeks.Workbench.Core.Management
{
    public class UserspaceManager
    {
        public void SwitchUserspace()
        {
            SystemVariable.UserspaceDirectory = GetUserspaceDirectory();
        }

        public string GetUserspaceDirectory()
        {
            return Path.Combine(FsPath.UserDirectory(), Directories.Workbench);
        }
    }
}
