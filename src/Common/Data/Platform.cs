using System;
using System.IO;

namespace AstralKeks.Workbench.Common.Data
{
    public class Platform
    {
        public const string Windows = "Windows";
        public const string Unix = "Unix";

        public static string Current
        {
            get
            {
                switch (Path.DirectorySeparatorChar)
                {
                    case '/':
                        return Unix;
                    case '\\':
                        return Windows;
                    default:
                        throw new NotSupportedException("Unknown operating system");
                }
            }
        }
    }
}
