
namespace AstralKeks.Workbench.Core.Data
{
    internal static class FileSystem
    {
        public static string WorkbenchDirectory => ".Workbench";
        public static string ConfigDirectory => "Config";
        public static string SourceDirectory => "Source";
        public static string TempDirectory => "Temp";


        public static string LauncherFile => "Start-Workspace.bat";
        public static string ApplicationFile => "Application.json";
        public static string ToolkitFile => "Toolkit.json";
        public static string WorkspaceFile => "Workspace.json";
        public static string HistoryFile => "History.json";
        public static string WorkspaceMarkerFile => ".Workspace";
    }
}
