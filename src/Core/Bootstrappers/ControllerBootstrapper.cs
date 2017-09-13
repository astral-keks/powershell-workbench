using AstralKeks.Workbench.Controllers;
using Autofac;
using Autofac.Builder;
using Activator = Autofac.Builder.ConcreteReflectionActivatorData;
using Style = Autofac.Builder.SingleRegistrationStyle;

namespace AstralKeks.Workbench.Bootstrappers
{
    public class ControllerBootstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterApplicationController(builder).As<ApplicationController>().SingleInstance();
            RegisterToolkitController(builder).As<ToolkitController>().SingleInstance();
            RegisterUserspaceController(builder).As<UserspaceController>().SingleInstance();
            RegisterWorkspaceController(builder).As<WorkspaceController>().SingleInstance();

            base.Load(builder);
        }

        protected virtual IRegistrationBuilder<ApplicationController, Activator, Style> RegisterApplicationController(ContainerBuilder builder)
        {
            return builder.RegisterType<ApplicationController>();
        }

        protected virtual IRegistrationBuilder<ToolkitController, Activator, Style> RegisterToolkitController(ContainerBuilder builder)
        {
            return builder.RegisterType<ToolkitController>();
        }

        protected virtual IRegistrationBuilder<UserspaceController, Activator, Style> RegisterUserspaceController(ContainerBuilder builder)
        {
            return builder.RegisterType<UserspaceController>();
        }

        protected virtual IRegistrationBuilder<WorkspaceController, Activator, Style> RegisterWorkspaceController(ContainerBuilder builder)
        {
            return builder.RegisterType<WorkspaceController>();
        }
    }
}
