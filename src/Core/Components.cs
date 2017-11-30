using AstralKeks.Workbench.Bootstrappers;
using AstralKeks.Workbench.Controllers;
using AstralKeks.Workbench.Repositories;
using Autofac;

namespace AstralKeks.Workbench
{
    public class ComponentContainer
    {
        private readonly IContainer _container;

        public ComponentContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new InfrastructureBootstrapper());
            builder.RegisterModule(new ContextBootstrapper());
            builder.RegisterModule(new RepositoryBootstrapper());
            builder.RegisterModule(new ControllerBootstrapper());
            builder.RegisterType<ResourceBootstrapper>().As<IStartable>().SingleInstance();
            _container = builder.Build();
        }

        public ApplicationController ApplicationController => _container.Resolve<ApplicationController>();
        public ToolkitController ToolkitСontroller => _container.Resolve<ToolkitController>();
        public UserspaceController UserspaceController => _container.Resolve<UserspaceController>();
        public WorkspaceController WorkspaceController => _container.Resolve<WorkspaceController>();
        public ShortcutController ShortcutController => _container.Resolve<ShortcutController>();
        public TemplateController TemplateController => _container.Resolve<TemplateController>();
        public BackupController BackupController => _container.Resolve<BackupController>();

        public ApplicationRepository ApplicationRepository => _container.Resolve<ApplicationRepository>();
        public ShortcutRepository ShortcutRepository => _container.Resolve<ShortcutRepository>();
        public UserspaceRepository UserspaceRepository => _container.Resolve<UserspaceRepository>();
        public WorkspaceRepository WorkspaceRepository => _container.Resolve<WorkspaceRepository>();
        public BackupRepository BackupRepository => _container.Resolve<BackupRepository>();
    }
}
