using AstralKeks.Workbench.Bootstrappers;
using AstralKeks.Workbench.Controllers;
using AstralKeks.Workbench.Repositories;
using Autofac;
using Autofac.Core;
using System;

namespace AstralKeks.Workbench
{
    public class ComponentContainer
    {
        private readonly IContainer _container;

        public ComponentContainer() : this(
            new InfrastructureBootstrapper(),
            new ContextBootstrapper(),
            new RepositoryBootstrapper(),
            new ControllerBootstrapper())
        {
        }

        public ComponentContainer(IModule infrastructureModule, IModule contextModule, IModule repositoryModule, 
            IModule controllerModule)
        {
            if (infrastructureModule == null)
                throw new ArgumentNullException(nameof(infrastructureModule));
            if (contextModule == null)
                throw new ArgumentNullException(nameof(contextModule));
            if (repositoryModule == null)
                throw new ArgumentNullException(nameof(repositoryModule));
            if (controllerModule == null)
                throw new ArgumentNullException(nameof(controllerModule));

            var builder = new ContainerBuilder();
            builder.RegisterModule(infrastructureModule);
            builder.RegisterModule(contextModule);
            builder.RegisterModule(repositoryModule);
            builder.RegisterModule(controllerModule);
            _container = builder.Build();
        }

        public ApplicationController ApplicationController => _container.Resolve<ApplicationController>();
        public ToolkitController ToolkitСontroller => _container.Resolve<ToolkitController>();
        public UserspaceController UserspaceController => _container.Resolve<UserspaceController>();
        public WorkspaceController WorkspaceController => _container.Resolve<WorkspaceController>();
        public ShortcutController ShortcutController => _container.Resolve<ShortcutController>();
        public TemplateController TemplateController => _container.Resolve<TemplateController>();

        public ApplicationRepository ApplicationRepository => _container.Resolve<ApplicationRepository>();
        public ShortcutRepository ShortcutRepository => _container.Resolve<ShortcutRepository>();
        public UserspaceRepository UserspaceRepository => _container.Resolve<UserspaceRepository>();
        public WorkspaceRepository WorkspaceRepository => _container.Resolve<WorkspaceRepository>();
    }
}
