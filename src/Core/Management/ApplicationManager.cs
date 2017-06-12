using AstralKeks.Workbench.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AstralKeks.Workbench.Core.Management
{
    public class ApplicationManager
    {
        private readonly WorkspaceManager _workspaceManager;
        private readonly UserspaceManager _userspaceManager;
        private readonly ConfigurationManager _configurationManager;
        private readonly MacrosManager _macrosManager;

        public ApplicationManager(WorkspaceManager workspaceManager, UserspaceManager userspaceManager,
            ConfigurationManager configurationManager, MacrosManager macrosManager)
        {
            _workspaceManager = workspaceManager ?? throw new ArgumentNullException(nameof(workspaceManager));
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
            _macrosManager = macrosManager ?? throw new ArgumentNullException(nameof(macrosManager));
        }

        public Application[] GetApplications()
        {
            var workspaceDirectory = _workspaceManager.GetWorkspaceDirectory();
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            return _configurationManager.GetApplicationConfig(workspaceDirectory, userspaceDirectory);
        }

        public Command[] GetCommands(string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
                throw new ArgumentException("Value is empty", nameof(applicationName));

            return GetApplications()
               .Where(app => ApplicationHasName(app, applicationName))
               .SelectMany(app => app.Commands)
               .Distinct()
               .ToArray();
        }

        public void StartApplication(string applicationName = null, string commandName = null, 
            List<string> arguments = null, string pipeline = null)
        {
            applicationName = applicationName ?? Application.Default;
            commandName = commandName ?? Command.Default;
            arguments = (arguments ?? Enumerable.Empty<string>()).Select(a => $"\"{a}\"").ToList();
            pipeline = !string.IsNullOrEmpty(pipeline) ? $"\"{pipeline ?? string.Empty}\"" : string.Empty;

            var applications = GetApplications();
            var application = applications.FirstOrDefault(app => ApplicationHasName(app, applicationName));
            var command = application?.Commands.FirstOrDefault(cmd => CommandHasName(cmd, commandName));
            var commandArguments = (command?.Arguments ?? new List<string>())
                .Select(a => _macrosManager.ResolveMacros(a, pipeline, arguments))
                .Where(a => !string.IsNullOrEmpty(a))
                .ToList();

            if (application == null)
                throw new ArgumentException($"Application '{applicationName}' is not configured");
            if (command == null)
                throw new ArgumentException($"Command '{commandName}' in application '{applicationName}' is not configured");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = application.Executable,
                    Arguments = string.Join(" ", commandArguments),
                    UseShellExecute = command.UseShellExecute,
                    CreateNoWindow = command.NoWindow
                }
            };

            #region Unsupported

            //if (command.RereadEnvironment)
            //{
            //    var variables = Enumerable.Empty<DictionaryEntry>()
            //        .Concat(Environment.GetEnvironmentVariables().Cast<DictionaryEntry>())
            //        .Concat(Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine).Cast<DictionaryEntry>())
            //        .Concat(Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User).Cast<DictionaryEntry>());

            //    foreach (var variable in variables)
            //    {
            //        var name = variable.Key as string;
            //        var value = variable.Value as string;

            //        if (name == "Path")
            //        {
            //            var path = string.Join(";", process.StartInfo.Environment[name], value);
            //            var pathParts = path.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
            //            process.StartInfo.Environment[name] = string.Join(";", pathParts);
            //        }
            //        else
            //        {
            //            process.StartInfo.Environment[name] = value;
            //        }
            //    }
            //}

            //if (command.RunAs)
            //    process.StartInfo.Verb = "runas";

            #endregion

            process.Start();
            if (command.WaitForExit)
                process.WaitForExit();
        }

        private bool ApplicationHasName(Application application, string applicationName)
        {
            return string.Equals(application.Name, applicationName, StringComparison.OrdinalIgnoreCase);
        }

        private bool CommandHasName(Command command, string commandName)
        {
            return string.Equals(command.Name, commandName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
