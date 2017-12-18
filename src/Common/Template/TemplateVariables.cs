using System.Collections.Generic;
using System.IO;

namespace AstralKeks.Workbench.Common.Template
{
    public partial struct TemplateVariable
    {
        public static TemplateVariable Workbench(string workbenchPath) => new TemplateVariable("Workbench", PreparePath(workbenchPath));

        public static TemplateVariable Userspace(string userspacePath) => new TemplateVariable("Userspace", PreparePath(userspacePath));

        public static TemplateVariable Workspace(string workspacePath) => new TemplateVariable("Workspace", PreparePath(workspacePath));
        
        public static TemplateVariable Args(IEnumerable<string> args) => new TemplateVariable("Args", PrepareList(args));

        public static TemplateVariable Pipeline(string pipeline) => new TemplateVariable("Pipeline", pipeline);
        
        private static string PreparePath(string path)
        {
            return path?.TrimEnd(Path.DirectorySeparatorChar) ?? string.Empty;
        }

        private static string PrepareList(IEnumerable<string> list)
        {
            return string.Join(" ", list ?? new string[0]);
        }
    }
}
