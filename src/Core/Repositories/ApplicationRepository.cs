using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Template;
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
        private readonly SharedContext _sharedContext;
        private readonly TemplateProcessor _templateProcessor;
        private readonly ResourceRepository _resourceRepository;

        public ApplicationRepository(SharedContext sharedContext, TemplateProcessor templateProcessor, 
            ResourceRepository resourceRepository)
        {
            _sharedContext = sharedContext ?? throw new ArgumentNullException(nameof(sharedContext));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public Application[] GetApplications()
        {
            var workspaceResourcePath = PathBuilder.Complete(
                _sharedContext.CurrentWorkspaceDirectory, Directories.Config, Directories.Workbench, Files.ApplicationJson);
            var userspaceResourcePath = PathBuilder.Combine(
                _sharedContext.CurrentUserspaceDirectory, Directories.Config, Directories.Workbench, Files.ApplicationJson);
            var workspaceResource = _templateProcessor.Transform(_resourceRepository.GetResource(workspaceResourcePath));
            var userspaceResource = _templateProcessor.Transform(_resourceRepository.GetResource(userspaceResourcePath));

            var applicationConfiguration = userspaceResource?.Read<ApplicationConfig>(workspaceResource);
            applicationConfiguration = applicationConfiguration ?? new ApplicationConfig();
            return applicationConfiguration.ToArray();
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
