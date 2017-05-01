
namespace AstralKeks.Workbench.Launcher
{
    internal static class Constants
    {
        public static class Commands
        {
            public const string Application = "application";
            public const string Workspace = "workspace";
            public const string Install = "install";
        }

        public static class Verbs
        {
            public const string Start = "start";
            public const string List = "list";
            public const string Create = "create";
        }

        public static class Arguments
        {
            public const string ApplicationName = "[name]";
            public const string ApplicationNameDesc = "application name";
            public const string CommandName = "[command]";
            public const string CommandNameDesc = "application command name";
            public const string ArgumentsList = "[args]";
            public const string ArgumentsListDesc = "application arguments";
        }

        public static class Options
        {
            public const string H = "-h";
            public const string Help = "--help";
            public const string Question = "-?";
        }

        public static readonly string HelpTemplate = $"{Options.H}|{Options.Help}|{Options.Question}";
    }
}
