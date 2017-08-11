using System.IO;
using static AstralKeks.Workbench.Common.FileSystem.FsPath;

namespace AstralKeks.Workbench.Common.Context
{
    public class Location
    {
        public static string Workspace(string directoryName = null)
        {
            return SystemVariable.WorkspaceDirectory ?? Absolute(Directory.GetCurrentDirectory(), directoryName);
        }

        public static string Userspace(string directoryName = null)
        {
            return SystemVariable.UserspaceDirectory ?? Absolute(UserDirectory(), directoryName);
        }
    }
}
