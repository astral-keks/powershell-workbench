using AstralKeks.Workbench.Fixtures;
using AstralKeks.Workbench.Resources;
using AstralKeks.Workbench.Controllers;
using AstralKeks.Workbench.Infrastructure;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Repositories;
using System.Linq;
using Xunit;
using AstralKeks.Workbench.Utilities;
using AstralKeks.Workbench.Context;
using System.Collections.Generic;
using System;
using AstralKeks.Workbench.Template;
using AutoFixture.Xunit2;

namespace AstralKeks.Workbench.Tests
{
    public class StartApplicationTests
    {
        [Theory(DisplayName = nameof(StartApplication_Plain_Success)), AutoSetup]
        public void StartApplication_Plain_Success(
            ApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher)
        {
            var application = applicationRepository.GetApplication(Application.Default);


            applicationController.StartApplication();


            Assert.Contains(processLauncher.Processes, p => p.FileName == application.Executable);
        }

        [Theory(DisplayName = nameof(StartApplication_ApplicationName_Success)), AutoSetup]
        public void StartApplication_ApplicationName_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            Application application)
        {
            application.Commands[new Random().Next(0, application.Commands.Count - 1)].Name = Command.Default;
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name);


            Assert.Contains(processLauncher.Processes, p => p.FileName == application.Executable);
        }

        [Theory(DisplayName = nameof(StartApplication_CommandName_Success)), AutoSetup]
        public void StartApplication_CommandName_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            Application application)
        {
            var command = application.Commands[new Random().Next(0, application.Commands.Count - 1)];
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name, command.Name);


            Assert.Contains(processLauncher.Processes, p => p.FileName == application.Executable);
        }

        [Theory(DisplayName = nameof(StartApplication_Arguments_Success)), AutoSetup]
        public void StartApplication_Arguments_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            Application application,
            List<string> arguments)
        {
            var argsVariable = TemplateVariable.Args(arguments);

            var command = application.Commands[new Random().Next(0, application.Commands.Count - 1)];
            command.Arguments = new List<string> { argsVariable.Name };
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name, command.Name, arguments);


            var process = processLauncher.Processes.FirstOrDefault(p => p.FileName == application.Executable);
            Assert.NotNull(process);
            Assert.All(arguments, argument => Assert.Contains(argument, process.Arguments));
        }

        [Theory(DisplayName = nameof(StartApplication_Pipeline_Success)), AutoSetup]
        public void StartApplication_Pipeline_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            Application application,
            string pipeline)
        {
            var pipelineVariable = TemplateVariable.Pipeline(pipeline);

            var command = application.Commands[new Random().Next(0, application.Commands.Count - 1)];
            command.Arguments = new List<string> { pipelineVariable.Name };
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name, command.Name, pipeline: pipeline);


            var process = processLauncher.Processes.FirstOrDefault(p => p.FileName == application.Executable);
            Assert.NotNull(process);
            Assert.Contains(pipeline, process.Arguments);
        }

        [Theory(DisplayName = nameof(StartApplication_NoWindow_Success)), AutoSetup]
        public void StartApplication_NoWindow_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            Application application,
            bool noWindow)
        {
            var command = application.Commands[new Random().Next(0, application.Commands.Count - 1)];
            command.NoWindow = noWindow;
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name, command.Name);


            var process = processLauncher.Processes.FirstOrDefault(p => p.FileName == application.Executable);
            Assert.NotNull(process);
            Assert.Equal(process.CreateNoWindow, noWindow);
        }

        [Theory(DisplayName = nameof(StartApplication_DeleteVariables_Success)), AutoSetup]
        public void StartApplication_DeleteVariables_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            SystemVariableMockup systemVariable,
            Application application,
            List<string> deleteVariables)
        {
            deleteVariables.ForEach(name => systemVariable.AddVariable(name, name));
            var command = application.Commands[new Random().Next(0, application.Commands.Count - 1)];
            command.DeleteVariables = deleteVariables.ToArray();
            command.RereadProcessVariables = false;
            command.RereadProcessVariables = false;
            command.RereadMachineVariables = false;
            command.ResetVariables = false;
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name, command.Name);


            var process = processLauncher.Processes.FirstOrDefault(p => p.FileName == application.Executable);
            Assert.NotNull(process);
            Assert.DoesNotContain(process.Environment.Keys, deleteVariables.Contains);
        }

        [Theory(DisplayName = nameof(StartApplication_RereadProcessVariables_Success)), AutoSetup]
        public void StartApplication_RereadProcessVariables_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            SystemVariableMockup systemVariable,
            Application application,
            List<(string Name, string Value)> processVariables)
        {
            processVariables.ForEach(v => systemVariable.AddVariable(v.Name, v.Value, EnvironmentVariableTarget.Process));
            var command = application.Commands[new Random().Next(0, application.Commands.Count - 1)];
            command.RereadProcessVariables = true;
            command.RereadMachineVariables = false;
            command.RereadUserVariables = false;
            command.ResetVariables = false;
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name, command.Name);


            var process = processLauncher.Processes.FirstOrDefault(p => p.FileName == application.Executable);
            Assert.NotNull(process);
            Assert.Contains(process.Environment, pv => processVariables.Any(v => v.Name == pv.Key && v.Value == pv.Value));
        }

        [Theory(DisplayName = nameof(StartApplication_RereadMachineVariables_Success)), AutoSetup]
        public void StartApplication_RereadMachineVariables_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            SystemVariableMockup systemVariable,
            Application application,
            List<(string Name, string Value)> machineVariables)
        {
            machineVariables.ForEach(v => systemVariable.AddVariable(v.Name, v.Value, EnvironmentVariableTarget.Machine));
            var command = application.Commands[new Random().Next(0, application.Commands.Count - 1)];
            command.RereadProcessVariables = false;
            command.RereadMachineVariables = true;
            command.RereadUserVariables = false;
            command.ResetVariables = false;
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name, command.Name);


            var process = processLauncher.Processes.FirstOrDefault(p => p.FileName == application.Executable);
            Assert.NotNull(process);
            Assert.Contains(process.Environment, pv => machineVariables.Any(v => v.Name == pv.Key && v.Value == pv.Value));
        }

        [Theory(DisplayName = nameof(StartApplication_RereadUserVariables_Success)), AutoSetup]
        public void StartApplication_RereadUserVariables_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            SystemVariableMockup systemVariable,
            Application application,
            List<(string Name, string Value)> userVariables)
        {
            userVariables.ForEach(v => systemVariable.AddVariable(v.Name, v.Value, EnvironmentVariableTarget.User));
            var command = application.Commands[new Random().Next(0, application.Commands.Count - 1)];
            command.RereadProcessVariables = false;
            command.RereadMachineVariables = false;
            command.RereadUserVariables = true;
            command.ResetVariables = false;
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name, command.Name);


            var process = processLauncher.Processes.FirstOrDefault(p => p.FileName == application.Executable);
            Assert.NotNull(process);
            Assert.Contains(process.Environment, pv => userVariables.Any(v => v.Name == pv.Key && v.Value == pv.Value));
        }

        [Theory(DisplayName = nameof(StartApplication_ResetVariables_Success)), AutoSetup]
        public void StartApplication_ResetVariables_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            Application application)
        {
            var command = application.Commands[new Random().Next(0, application.Commands.Count - 1)];
            command.RereadProcessVariables = false;
            command.RereadMachineVariables = false;
            command.RereadUserVariables = false;
            command.ResetVariables = true;
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name, command.Name);


            var process = processLauncher.Processes.FirstOrDefault(p => p.FileName == application.Executable);
            Assert.NotNull(process);
            Assert.Empty(process.Environment);
        }

        [Theory(DisplayName = nameof(StartApplication_UseShellExecute_Success)), AutoSetup]
        public void StartApplication_UseShellExecute_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            Application application,
            bool useShellExecute)
        {
            var command = application.Commands[new Random().Next(0, application.Commands.Count - 1)];
            command.UseShellExecute = useShellExecute;
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name, command.Name);


            var process = processLauncher.Processes.FirstOrDefault(p => p.FileName == application.Executable);
            Assert.NotNull(process);
            Assert.Equal(process.UseShellExecute, useShellExecute);
        }

        [Theory(DisplayName = nameof(StartApplication_WaitForExit_Success)), AutoSetup]
        public void StartApplication_WaitForExit_Success(
            TestApplicationRepository applicationRepository,
            ApplicationController applicationController,
            ProcessLauncherMockup processLauncher,
            Application application,
            bool waitForExit)
        {
            var command = application.Commands[new Random().Next(0, application.Commands.Count - 1)];
            command.WaitForExit = waitForExit;
            applicationRepository.AddApplication(application);


            applicationController.StartApplication(application.Name, command.Name);


            var process = processLauncher.Processes.FirstOrDefault(p => p.FileName == application.Executable);
            Assert.NotNull(process);
            if (waitForExit)
                Assert.Contains(process, processLauncher.Completed);
            else
                Assert.DoesNotContain(process, processLauncher.Completed);
        }
    }
}
