using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Macros;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AstralKeks.Workbench.Controllers
{
    public class ApplicationController
    {
        private readonly ApplicationRepository _applicationRepository;
        private readonly SystemVariable _systemVariable;
        private readonly ProcessLauncher _processLauncher;
        private readonly MacrosResolver _macrosResolver;

        public ApplicationController(FileSystem fileSystem, SystemVariable systemVariable, ProcessLauncher processLauncher,
            ApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
            _processLauncher = processLauncher ?? throw new ArgumentNullException(nameof(processLauncher));
            _macrosResolver = new MacrosResolver(fileSystem, systemVariable);
        }
        
        public void StartApplication(string applicationName = null, string commandName = null, 
            List<string> arguments = null, string pipeline = null)
        {
            applicationName = applicationName ?? Application.Default;
            commandName = commandName ?? Command.Default;
            arguments = (arguments ?? Enumerable.Empty<string>()).Select(a => $"\"{a}\"").ToList();
            pipeline = !string.IsNullOrEmpty(pipeline) ? $"\"{pipeline ?? string.Empty}\"" : string.Empty;

            var application = _applicationRepository.GetApplication(applicationName);
            var command = _applicationRepository.GetCommand(applicationName, commandName);
            var commandArguments = (command?.Arguments ?? new List<string>())
                .Select(a => _macrosResolver.ResolveMacros(a, pipeline, arguments))
                .Where(a => !string.IsNullOrEmpty(a))
                .ToList();

            if (application == null)
                throw new ArgumentException($"Application '{applicationName}' is not configured");
            if (command == null)
                throw new ArgumentException($"Command '{commandName}' in application '{applicationName}' is not configured");

            var processInfo = GetProcessInfo(application, command, commandArguments);
            _processLauncher.Launch(processInfo, command.WaitForExit);
            
        }

        private ProcessStartInfo GetProcessInfo(Application application, Command command, List<string> arguments)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = application.Executable,
                Arguments = string.Join(" ", arguments),
                UseShellExecute = command.UseShellExecute,
                CreateNoWindow = command.NoWindow
            };
            
            if (command.ResetVariables)
                processInfo.Environment.Clear();

            if (command.DeleteVariables?.Any() == true)
            {
                foreach (var variableName in command.DeleteVariables)
                    processInfo.Environment.Remove(variableName);
            }

            if (command.RereadCurrentVariables || command.RereadMachineVariables || command.RereadUserVariables)
            {
                var variables = Enumerable.Empty<DictionaryEntry>();
                if (command.RereadCurrentVariables)
                    AppendProcessVariables(processInfo, _systemVariable.GetVariables());
                if (command.RereadMachineVariables)
                    AppendProcessVariables(processInfo, _systemVariable.GetVariables(EnvironmentVariableTarget.Machine));
                if (command.RereadUserVariables)
                    AppendProcessVariables(processInfo, _systemVariable.GetVariables(EnvironmentVariableTarget.User));
            }

            return processInfo;
        }
        
        private void AppendProcessVariables(ProcessStartInfo processInfo, IDictionary variables)
        {
            var environment = processInfo.Environment;
            foreach (var variable in variables.Cast<DictionaryEntry>())
            {
                var name = variable.Key as string;
                var value = variable.Value as string;

                if (name == "Path")
                {
                    var path = string.Join(";", environment.ContainsKey(name) ? environment[name] : string.Empty, value);
                    var pathParts = path.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    processInfo.Environment[name] = string.Join(";", pathParts);
                }
                else
                {
                    processInfo.Environment[name] = value;
                }
            }
        }
    }
}
