using System;
using System.IO;

namespace AstralKeks.Workbench.Core.Management
{
    public class OperatingSystemManager
    {
        public const string Windows = "Windows";
        public const string Linux = "Linux";

        public static string CurrentOS
        {
            get
            {
                switch (Path.DirectorySeparatorChar)
                {
                    case '/':
                        return Linux;
                    case '\\':
                        return Windows;
                    default:
                        throw new NotSupportedException("Unknown operating system");
                }
            }
        }

    }
}
