using AstralKeks.Workbench.Core;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.IO;
using static AstralKeks.Workbench.Launcher.Constants;

namespace AstralKeks.Workbench.Launcher
{
    public class Program
    {
        public static readonly int SuccessCode = 0;
        public static readonly int FailureCode = 1;

        public static int Main(string[] args)
        {
            var app = new CommandLineApplication()
            {
                Name = Names.ApplicationName
            };

            app.OnExecute(() => HandleHelp(app));

            app.Command(Nouns.Environment, noun =>
            {
                noun.Command(Verbs.Install, verb =>
                {
                    var quiet = verb.Option(Options.QuietTemplate, Descriptions.QuietOption, CommandOptionType.NoValue);
                    verb.OnExecute(() => EnvironmentInstall(quiet));
                    verb.Description = Descriptions.EnvironmentInstallVerb;
                    verb.HelpOption(Options.HelpTemplate);
                });
                noun.Command(Verbs.Uninstall, verb =>
                {
                    var quiet = verb.Option(Options.QuietTemplate, Descriptions.QuietOption, CommandOptionType.NoValue);
                    verb.OnExecute(() => EnvironmentUninstall(quiet));
                    verb.Description = Descriptions.EnvironmentUninstallVerb;
                    verb.HelpOption(Options.HelpTemplate);
                });
                noun.Command(Verbs.Reset, verb =>
                {
                    var quiet = verb.Option(Options.QuietTemplate, Descriptions.QuietOption, CommandOptionType.NoValue);
                    verb.OnExecute(() => EnvironmentReset(quiet));
                    verb.Description = Descriptions.EnvironmentResetVerb;
                    verb.HelpOption(Options.HelpTemplate);
                });
                noun.OnExecute(() => HandleHelp(noun));
                noun.HelpOption(Options.HelpTemplate);
            });

            app.Command(Nouns.Workspace, noun =>
            {
                noun.Command(Verbs.Create, verb =>
                {
                    var quiet = verb.Option(Options.QuietTemplate, Descriptions.QuietOption, CommandOptionType.NoValue);
                    verb.OnExecute(() => WorkspaceCreate(quiet));
                    verb.Description = Descriptions.WorkspaceCreateVerb;
                    verb.HelpOption(Options.HelpTemplate);
                });
                noun.Command(Verbs.Start, verb =>
                {
                    var appName = verb.Argument(Arguments.ApplicationName, Descriptions.ApplicationNameArg);
                    var argList = verb.Argument(Arguments.ArgumentsList, Descriptions.ArgumentsArg, true);
                    var quiet = verb.Option(Options.QuietTemplate, Descriptions.QuietOption, CommandOptionType.NoValue);
                    verb.OnExecute(() => WorkspaceStart(appName, argList, quiet));
                    verb.Description = Descriptions.WorkspaceStartVerb;
                    verb.HelpOption(Options.HelpTemplate);
                });
                noun.OnExecute(() => HandleHelp(noun));
                noun.HelpOption(Options.HelpTemplate);
            });

            app.HelpOption(Options.HelpTemplate);

            try
            {
                return app.Execute(args);
            }
            catch (Exception e)
            {
                return HandleError(e);
            }
        }
        
        private static int EnvironmentInstall(CommandOption quiet)
        {
            var host = new WorkbenchHost();
            host.InstallEnvironment(() => quiet.HasValue() || Prompt.YesNo(Messages.InstallEnvironment));

            return HandleSuccess(Messages.Success, quiet);
        }

        private static int EnvironmentUninstall(CommandOption quiet)
        {
            var host = new WorkbenchHost();
            host.UninstallEnvironment(() => quiet.HasValue() || Prompt.YesNo(Messages.UninstallEnvironment));

            return HandleSuccess(Messages.Success, quiet);
        }

        private static int EnvironmentReset(CommandOption quiet)
        {
            var host = new WorkbenchHost();
            host.ResetEnvironment(() => quiet.HasValue() || Prompt.YesNo(Messages.ResetEnvironment));

            return HandleSuccess(Messages.Success, quiet);
        }

        private static int WorkspaceStart(CommandArgument applicationName, CommandArgument args, CommandOption quiet)
        {
            var host = new WorkbenchHost();
            host.StartWorkspace(applicationName.Value, args.Values, 
                d => quiet.HasValue() || Prompt.YesNo(string.Format(Messages.WorkspaceStart, d)));

            return HandleSuccess(null, quiet);
        }

        private static int WorkspaceCreate(CommandOption quiet)
        {
            var host = new WorkbenchHost();
            host.CreateWorkspace(d => quiet.HasValue() || Prompt.YesNo(string.Format(Messages.WorkspaceCreate, d)));

            return HandleSuccess(Messages.Success, quiet);
        }

        private static int HandleSuccess(string message, CommandOption quiet)
        {
            if (!string.IsNullOrWhiteSpace(message) && !quiet.HasValue())
                Console.WriteLine(message);
            return SuccessCode;
        }

        private static int HandleHelp(CommandLineApplication app)
        {
            app.ShowHelp();
            return FailureCode;
        }

        private static int HandleError(Exception e)
        {
            var msg = e.Message
                .Replace($".{Environment.NewLine}", ". ")
                .Replace(Environment.NewLine, ". ");
            Console.WriteLine($"ERROR: {msg}");
            return FailureCode;
        }
    }
}