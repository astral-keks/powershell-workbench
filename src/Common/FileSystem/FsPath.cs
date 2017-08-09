using AstralKeks.Workbench.Common.Data;
using System;
using System.IO;
using System.Reflection;

namespace AstralKeks.Workbench.Common.FileSystem
{
    public class FsPath
    {
        public static string UserDirectory()
        {
            switch (Platform.Current)
            {
                case Platform.Windows:
                    return SystemVariable.LocalAppData;
                case Platform.Unix:
                    return SystemVariable.Home;
                default:
                    throw new NotSupportedException();
            }
        }

        public static string BinDirectory()
        {
            var codeBase = typeof(FsPath).GetTypeInfo().Assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
        
        public static string Absolute(string parentPath, string middlePath, string childPath)
        {
            parentPath = Absolute(parentPath, middlePath);
            return Absolute(parentPath, childPath);
        }

        public static string Absolute(string parentPath, string childPath = null)
        {
            var combinedPath = Path.Combine(parentPath, childPath ?? string.Empty);
            if (!Path.IsPathRooted(combinedPath))
                combinedPath = Path.Combine(Directory.GetCurrentDirectory(), combinedPath);
            return Path.GetFullPath(new Uri(combinedPath).LocalPath);
        }
    }
}
