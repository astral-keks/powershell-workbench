using Autofac;
using Autofac.Builder;
using AstralKeks.Workbench.Common.Infrastructure;
using Style = Autofac.Builder.SingleRegistrationStyle;
using Activator = Autofac.Builder.ConcreteReflectionActivatorData;
using SimpleActivator = Autofac.Builder.SimpleActivatorData;

namespace AstralKeks.Workbench.Bootstrappers
{
    public class InfrastructureBootstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterFileSystem(builder).As<FileSystem>().SingleInstance();
            RegisterSystemVariable(builder).As<SystemVariable>().SingleInstance();
            RegisterResourceBundle(builder).As<ResourceBundle>().SingleInstance();
            RegisterProcessLauncher(builder).As<ProcessLauncher>().SingleInstance();

            base.Load(builder);
        }

        protected virtual IRegistrationBuilder<FileSystem, Activator, Style> RegisterFileSystem(ContainerBuilder builder)
        {
            return builder.RegisterType<FileSystem>();
        }

        protected virtual IRegistrationBuilder<SystemVariable, Activator, Style> RegisterSystemVariable(ContainerBuilder builder)
        {
            return builder.RegisterType<SystemVariable>();
        }

        protected virtual IRegistrationBuilder<ResourceBundle, SimpleActivator, Style> RegisterResourceBundle(ContainerBuilder builder)
        {
            return builder.Register(b => new ResourceBundle(typeof(ComponentContainer)));
        }

        protected virtual IRegistrationBuilder<ProcessLauncher, Activator, Style> RegisterProcessLauncher(ContainerBuilder builder)
        {
            return builder.RegisterType<ProcessLauncher>();
        }
    }
}
