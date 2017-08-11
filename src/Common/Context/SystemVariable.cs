using System;

namespace AstralKeks.Workbench.Common.Context
{
    public class SystemVariable
    {
        public static string WorkspaceDirectory
        {
            get { return Environment.GetEnvironmentVariable("WBWorkspaceDirectory"); }
            set { Environment.SetEnvironmentVariable("WBWorkspaceDirectory", value); }
        }

        public static string UserspaceDirectory
        {
            get { return Environment.GetEnvironmentVariable("WBUserspaceDirectory"); }
            set { Environment.SetEnvironmentVariable("WBUserspaceDirectory", value); }
        }

        public static string Path
        {
            get { return Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User); }
            set { Environment.SetEnvironmentVariable("Path", value, EnvironmentVariableTarget.User); }
        }

        public static string LocalAppData
        {
            get { return Environment.GetEnvironmentVariable("LOCALAPPDATA"); }
        }

        public static string Home
        {
            get { return Environment.GetEnvironmentVariable("HOME"); }
        }
    }
}
