
namespace AstralKeks.Workbench.Launcher
{
    internal static class Constants
    {
        public static class Names
        {
            public const string ApplicationName = "workbench";
        }

        public static class Messages
        {
            public const string InstallEnvironment = "Install workbench environment variables and configurations?";
            public const string UninstallEnvironment = "Uninstall workbench environment variables and configurations?";
            public const string ResetEnvironment = "Reset workbench environment variables and configurations?";
            public const string WorkspaceStart = "Workspace was not found. Create workspace in {0}?";
            public const string WorkspaceCreate = "Create workspace in {0}?";
            public const string Success = "Success";
        }

        public static class Descriptions
        {
            public const string EnvironmentInstallVerb = "Install Workbench environment variables and configurations";
            public const string EnvironmentUninstallVerb = "Uninstall Workbench environment variables and configurations";
            public const string EnvironmentResetVerb = "Reset Workbench environment variables and configurations to defaults";

            public const string WorkspaceCreateVerb = "Create workspace in current directory";
            public const string WorkspaceStartVerb = "Find or create workspace and start application in current directory";

            public const string ApplicationNameArg = "application name";
            public const string ArgumentsArg = "application arguments";

            public const string QuietOption = "produce no prompts or output";
        }

        public static class Nouns
        {
            public const string Environment = "environment";
            public const string Workspace = "workspace";
        }
        
        public static class Verbs
        {
            public const string Install = "install";
            public const string Uninstall = "uninstall";
            public const string Reset = "reset";
            public const string Start = "start";
            public const string Create = "create";
        }
        
        public static class Arguments
        {
            public const string ApplicationName = "[app]";
            public const string ArgumentsList = "[args]";
        }

        public static class Options
        {
            public const string H = "-h";
            public const string Help = "--help";
            public const string Question = "-?";
            public static readonly string HelpTemplate = $"{H}|{Help}|{Question}";

            public const string Q = "-q";
            public const string Quiet = "--quiet";
            public static readonly string QuietTemplate = $"{Q}|{Quiet}";
        }
    }
}
