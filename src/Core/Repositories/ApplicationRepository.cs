using AstralKeks.Workbench.Common.Configuration;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Resources;
using System;
using System.Linq;

namespace AstralKeks.Workbench.Repositories
{
    public class ApplicationRepository
    {
        private readonly UserspaceContext _userspaceContext;
        private readonly WorkspaceContext _workspaceContext;
        private readonly ConfigurationProvider _configurationProvider;

        public ApplicationRepository(UserspaceContext userspaceContext, WorkspaceContext workspaceContext,
            ConfigurationProvider configurationProvider)
        {
            _userspaceContext = userspaceContext ?? throw new ArgumentNullException(nameof(userspaceContext));
            _workspaceContext = workspaceContext ?? throw new ArgumentNullException(nameof(workspaceContext));
            _configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        public Application[] GetApplications()
        {
            var userspaceConfigPath = PathBuilder.Combine(
                _userspaceContext.CurrentUserspaceDirectory, Directories.Config, Files.ApplicationUSJson);
            var workspaceConfigPath = PathBuilder.Complete(
                _workspaceContext.CurrentWorkspaceDirectory, Directories.Config, Files.ApplicationWSJson);
            return _configurationProvider.Get<Application[]>(userspaceConfigPath, workspaceConfigPath);
        }

        public Application GetApplication(string applicationName)
        {
            return GetApplications().FirstOrDefault(app => ApplicationHasName(app, applicationName));
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

        public Command GetCommand(string applicationName, string commandName)
        {
            var application = GetApplication(applicationName);
            return application?.Commands.FirstOrDefault(cmd => CommandHasName(cmd, commandName));
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
