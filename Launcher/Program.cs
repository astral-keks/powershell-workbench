using AstralKeks.Workbench.Core;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Linq;
using static AstralKeks.Workbench.Launcher.Constants;

namespace AstralKeks.Workbench.Launcher
{
    public class Program
    {
        public static readonly int SuccessCode = 0;
        public static readonly int FailureCode = 1;

        public static int Main(string[] args)
        {
            var cli = new CommandLineApplication();

            cli.Command(Commands.Install, ws =>
            {
                ws.OnExecute(() =>Install());
                ws.HelpOption(HelpTemplate);
            });
            cli.Command(Commands.Application, app =>
            {
                app.Command(Verbs.List, appList => appList.OnExecute(() => ApplicationList()));
                app.Command(Verbs.Start, appStart => {
                    var appName = appStart.Argument(Arguments.ApplicationName, Arguments.ApplicationNameDesc);
                    var cmdName = appStart.Argument(Arguments.CommandName, Arguments.CommandNameDesc);
                    var argList = appStart.Argument(Arguments.ArgumentsList, Arguments.ArgumentsListDesc, true);
                    appStart.OnExecute(() => ApplicationStart(appName, cmdName, argList));
                });
                app.HelpOption(HelpTemplate);
            });
            cli.Command(Commands.Workspace, ws =>
            {
                ws.Command(Verbs.Create, wsCreate => wsCreate.OnExecute(() => WorkspaceCreate()));
                ws.HelpOption(HelpTemplate);
            });

            cli.OnExecute(() => ApplicationStart());
            cli.HelpOption(HelpTemplate);

            try
            {
                return cli.Execute(args);
            }
            catch (Exception e)
            {
                return HandleError(e);
            }
        }

        private static int Install()
        {
            var host = new WorkbenchHost();
            Console.WriteLine($"{host.Install()} - installed");
            return SuccessCode;
        }

        private static int ApplicationList()
        {
            var host = new WorkbenchHost();
            var applications = host.ListApplications();
            applications.ForEach(Console.WriteLine);
            return SuccessCode;
        }

        private static int ApplicationStart(CommandArgument applicationName = null, CommandArgument commandName = null, 
            CommandArgument args = null)
        {
            var host = new WorkbenchHost();
            host.StartApplication(applicationName?.Value, commandName?.Value, args?.Values);
            return SuccessCode;
        }

        private static int WorkspaceCreate()
        {
            var host = new WorkbenchHost();
            host.CreateWorkspace();
            return SuccessCode;
        }

        private static int HandleError(Exception e)
        {
            Console.WriteLine(e.Message);
            return FailureCode;
        }
    }
}