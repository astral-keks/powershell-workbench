using System.IO;
using static AstralKeks.Workbench.Common.FileSystem.FsPath;

namespace AstralKeks.Workbench.Common.Data
{
    public class Location
    {
        public string Workspace(string directoryName = null)
        {
            return SystemVariable.WorkspaceDirectory ?? Absolute(Directory.GetCurrentDirectory(), directoryName);
        }

        public string Userspace(string directoryName = null)
        {
            return SystemVariable.UserspaceDirectory ?? Absolute(UserDirectory(), directoryName);
        }
    }
}
