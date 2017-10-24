using AstralKeks.Workbench.Repositories;
using Autofac;
using Autofac.Builder;
using Activator = Autofac.Builder.ConcreteReflectionActivatorData;
using Style = Autofac.Builder.SingleRegistrationStyle;

namespace AstralKeks.Workbench.Bootstrappers
{
    public class RepositoryBootstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterApplicationRepository(builder).As<ApplicationRepository>().SingleInstance();
            RegisterShortcutRepository(builder).As<ShortcutRepository>().SingleInstance();
            RegisterUserspaceRepository(builder).As<UserspaceRepository>().SingleInstance();
            RegisterWorkspaceRepository(builder).As<WorkspaceRepository>().SingleInstance();

            base.Load(builder);
        }

        protected virtual IRegistrationBuilder<ApplicationRepository, Activator, Style> RegisterApplicationRepository(ContainerBuilder builder)
        {
            return builder.RegisterType<ApplicationRepository>();
        }

        protected virtual IRegistrationBuilder<ShortcutRepository, Activator, Style> RegisterShortcutRepository(ContainerBuilder builder)
        {
            return builder.RegisterType<ShortcutRepository>();
        }

        protected virtual IRegistrationBuilder<UserspaceRepository, Activator, Style> RegisterUserspaceRepository(ContainerBuilder builder)
        {
            return builder.RegisterType<UserspaceRepository>();
        }

        protected virtual IRegistrationBuilder<WorkspaceRepository, Activator, Style> RegisterWorkspaceRepository(ContainerBuilder builder)
        {
            return builder.RegisterType<WorkspaceRepository>();
        }
    }
}
