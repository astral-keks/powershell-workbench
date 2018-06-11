using AstralKeks.Workbench.Context;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Resources;
using AstralKeks.Workbench.Template;
using System;
using System.Collections.Generic;

namespace AstralKeks.Workbench.Repositories
{
    public class TestApplicationRepository : ApplicationRepository
    {
        private readonly ResourceRepository _resourceRepository;

        public TestApplicationRepository(SessionContext sessionContext, TemplateProcessor templateProcessor, 
            ResourceRepository resourceRepository) 
            : base(sessionContext, templateProcessor, resourceRepository)
        {
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public void AddApplication(Application application)
        {
            var resource = _resourceRepository.GetResource(WorkspaceResourcePath);

            var applications = resource.Read<List<Application>>();
            applications.Add(application);
            resource.Write(applications);
        }
    }
}
