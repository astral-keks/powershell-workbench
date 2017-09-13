using Autofac;
using Autofac.Builder;
using AstralKeks.Workbench.Common.Infrastructure;
using Style = Autofac.Builder.SingleRegistrationStyle;
using Activator = Autofac.Builder.ConcreteReflectionActivatorData;

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

        protected virtual IRegistrationBuilder<ResourceBundle, Activator, Style> RegisterResourceBundle(ContainerBuilder builder)
        {
            return builder.RegisterType<ResourceBundle>();
        }

        protected virtual IRegistrationBuilder<ProcessLauncher, Activator, Style> RegisterProcessLauncher(ContainerBuilder builder)
        {
            return builder.RegisterType<ProcessLauncher>();
        }
    }
}
