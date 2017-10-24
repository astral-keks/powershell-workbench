using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Configuration;
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
        private readonly ResourceRepository _resourceRepository;

        public ApplicationRepository(WorkspaceContext workspaceContext, UserspaceContext userspaceContext, 
            ResourceRepository resourceRepository)
        {
            _workspaceContext = workspaceContext ?? throw new ArgumentNullException(nameof(workspaceContext));
            _userspaceContext = userspaceContext ?? throw new ArgumentNullException(nameof(userspaceContext));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public Application[] GetApplications()
        {
            var userspaceResourcePath = PathBuilder.Combine(
                _userspaceContext.CurrentUserspaceDirectory, Directories.Config, Files.WBApplicationJson);
            var workspaceResourcePath = PathBuilder.Combine(
                _workspaceContext.CurrentWorkspaceDirectory, Directories.Config, Files.WBApplicationJson);
            var userspaceResource = _resourceRepository.GetResource(userspaceResourcePath);
            var workspaceResource = _resourceRepository.GetResource(userspaceResourcePath);
            if (userspaceResource == null)
                throw new InvalidOperationException("Unable to get application configuration");

            return userspaceResource.Read<ApplicationConfig>(workspaceResource).ToArray();
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
