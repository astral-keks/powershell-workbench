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
                    verb.OnExecute(() => EnvironmentInstall());
                    verb.Description = Descriptions.EnvironmentInstallVerb;
                    verb.HelpOption(Options.HelpTemplate);
                });
                noun.Command(Verbs.Uninstall, verb =>
                {
                    verb.OnExecute(() => EnvironmentUninstall());
                    verb.Description = Descriptions.EnvironmentUninstallVerb;
                    verb.HelpOption(Options.HelpTemplate);
                });
                noun.Command(Verbs.Reset, verb =>
                {
                    verb.OnExecute(() => EnvironmentReset());
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
                    verb.OnExecute(() => WorkspaceCreate());
                    verb.Description = Descriptions.WorkspaceCreateVerb;
                    verb.HelpOption(Options.HelpTemplate);
                });
                noun.Command(Verbs.Start, verb =>
                {
                    var appName = verb.Argument(Arguments.ApplicationName, Descriptions.ApplicationNameArg);
                    var argList = verb.Argument(Arguments.ArgumentsList, Descriptions.ArgumentsArg, true);
                    verb.OnExecute(() => WorkspaceStart(appName, argList));
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

        private static int EnvironmentInstall()
        {
            var host = new WorkbenchHost();
            host.InstallEnvironment();

            return HandleSuccess();
        }

        private static int EnvironmentUninstall()
        {
            var host = new WorkbenchHost();
            host.UninstallEnvironment();

            return HandleSuccess();
        }

        private static int EnvironmentReset()
        {
            var host = new WorkbenchHost();
            host.ResetEnvironment();

            return HandleSuccess();
        }

        private static int WorkspaceStart(CommandArgument applicationName, CommandArgument args)
        {
            var host = new WorkbenchHost();
            host.StartWorkspace(applicationName.Value, args.Values);

            return HandleSuccess();
        }

        private static int WorkspaceCreate()
        {
            var host = new WorkbenchHost();
            host.CreateWorkspace();

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