
namespace AstralKeks.Workbench.Core.Data
{
    internal static class FileSystem
    {
        public static string WorkbenchDirectory => ".Workbench";
        public static string ConfigDirectory => "Config";

        public static string ApplicationFile => "Application.json";
        public static string ToolkitFile => "Toolkit.json";
        public static string WorkspaceFile => "Workspace.json";
        public static string HistoryFile => "History.json";
        public static string WorkspaceLauncherFile => "Workspace.bat";
    }
}
