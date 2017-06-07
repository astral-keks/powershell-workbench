
namespace AstralKeks.Workbench.Launcher
{
    internal static class Constants
    {
        public static class Description
        {
            public const string Default = "Start workbench terminal in current workspace";

            public const string InstallVerb = "Add workbench to PATH variable";
            public const string StartVerb = "Start application in current workspace";
            public const string CreateVerb = "Create workspace in current directory";

            public const string ApplicationNameArg = "application name";
            public const string CommandNameArg = "application command name";
            public const string ArgumentsArg = "application arguments";
        }

        public static class Nouns
        {
            public const string Environment = "environment";
            public const string Application = "application";
            public const string Workspace = "workspace";
        }
        
        public static class Verbs
        {
            public const string Install = "install";
            public const string Start = "start";
            public const string Create = "create";
        }
        
        public static class Arguments
        {
            public const string ApplicationName = "[name]";
            public const string CommandName = "[command]";
            public const string ArgumentsList = "[args]";
        }

        public static class Options
        {
            public const string H = "-h";
            public const string Help = "--help";
            public const string Question = "-?";
            public static readonly string HelpTemplate = $"{Options.H}|{Options.Help}|{Options.Question}";
        }
    }
}
