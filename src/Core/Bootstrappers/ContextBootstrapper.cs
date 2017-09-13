using AstralKeks.Workbench.Common.Context;
using Autofac;
using Autofac.Builder;
using Style = Autofac.Builder.SingleRegistrationStyle;
using Activator = Autofac.Builder.ConcreteReflectionActivatorData;
using SimpleActivator = Autofac.Builder.SimpleActivatorData;
using AstralKeks.Workbench.Common.Resources;
using AstralKeks.Workbench.Common.Configuration;

namespace AstralKeks.Workbench.Bootstrappers
{
    public class ContextBootstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterUserspaceContext(builder).As<UserspaceContext>().SingleInstance();
            RegisterWorkspaceContext(builder).As<WorkspaceContext>().SingleInstance();
            RegisterResourceOrigin(builder).As<ResourceOrigin>().SingleInstance();
            RegisterResourceManager(builder).As<ResourceManager>().SingleInstance();
            RegisterConfigurationProvider(builder).As<ConfigurationProvider>().SingleInstance();
        }

        protected virtual IRegistrationBuilder<UserspaceContext, Activator, Style> RegisterUserspaceContext(ContainerBuilder builder)
        {
            return builder.RegisterType<UserspaceContext>();
        }

        protected virtual IRegistrationBuilder<WorkspaceContext, Activator, Style> RegisterWorkspaceContext(ContainerBuilder builder)
        {
            return builder.RegisterType<WorkspaceContext>();
        }

        protected virtual IRegistrationBuilder<ResourceOrigin, SimpleActivator, Style> RegisterResourceOrigin(ContainerBuilder builder)
        {
            return builder.Register(b => new ResourceOrigin(typeof(ComponentContainer))); ;
        }

        protected virtual IRegistrationBuilder<ResourceManager, Activator, Style> RegisterResourceManager(ContainerBuilder builder)
        {
            return builder.RegisterType<ResourceManager>();
        }

        protected virtual IRegistrationBuilder<ConfigurationProvider, Activator, Style> RegisterConfigurationProvider(ContainerBuilder builder)
        {
            return builder.RegisterType<ConfigurationProvider>();
        }
    }
}
