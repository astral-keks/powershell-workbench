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
            var app = new CommandLineApplication();
            app.OnExecute(() => Default());
            app.Command(Nouns.Environment, noun =>
            {
                noun.Command(Verbs.Install, verb =>
                {
                    verb.OnExecute(() => EnvironmentInstall());
                    verb.Description = Description.InstallVerb;
                    verb.HelpOption(Options.HelpTemplate);
                });
                noun.OnExecute(() => HandleHelp(noun));
                noun.HelpOption(Options.HelpTemplate);
            });
            app.Command(Nouns.Application, noun =>
            {
                noun.Command(Verbs.Start, verb => 
                {
                    var appName = verb.Argument(Arguments.ApplicationName, Description.ApplicationNameArg);
                    var cmdName = verb.Argument(Arguments.CommandName, Description.CommandNameArg);
                    var argList = verb.Argument(Arguments.ArgumentsList, Description.ArgumentsArg, true);
                    verb.OnExecute(() => ApplicationStart(appName, cmdName, argList));
                    verb.Description = Description.StartVerb;
                    verb.HelpOption(Options.HelpTemplate);
                });
                noun.OnExecute(() => HandleHelp(noun));
                noun.HelpOption(Options.HelpTemplate);
            });
            app.Command(Nouns.Workspace, noun =>
            {
                noun.Command(Verbs.Create, verb =>
                {
                    verb.OnExecute(() => WorkspaceCreate());
                    verb.Description = Description.CreateVerb;
                    verb.HelpOption(Options.HelpTemplate);
                });
                noun.OnExecute(() => HandleHelp(noun));
                noun.HelpOption(Options.HelpTemplate);
            });
            app.Description = Description.Default;
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

        private static int Default()
        {
            var host = new WorkbenchHost();
            host.StartDefaultApplication();
            return HandleSuccess();
        }


        private static int EnvironmentInstall()
        {
            var host = new WorkbenchHost();
            host.InstallEnvironment();

            Console.WriteLine("Environment installation succeeded");
            return HandleSuccess();
        }

        private static int ApplicationStart(CommandArgument applicationName, CommandArgument commandName, CommandArgument args)
        {
            var host = new WorkbenchHost();
            host.StartApplication(applicationName.Value, commandName.Value, args.Values);
            return HandleSuccess();
        }

        private static int WorkspaceCreate()
        {
            var host = new WorkbenchHost();
            host.CreateWorkspace();
            Console.WriteLine("Workspace creation succeeded");
            return HandleSuccess();
        }

        private static int HandleSuccess()
        {
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