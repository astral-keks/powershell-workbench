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
            RegisterShortcutController(builder).As<ShortcutController>().SingleInstance();
            RegisterTemplateController(builder).As<TemplateController>().SingleInstance();
            RegisterBackupController(builder).As<BackupController>().SingleInstance();

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

        protected virtual IRegistrationBuilder<ShortcutController, Activator, Style> RegisterShortcutController(ContainerBuilder builder)
        {
            return builder.RegisterType<ShortcutController>();
        }

        protected virtual IRegistrationBuilder<TemplateController, Activator, Style> RegisterTemplateController(ContainerBuilder builder)
        {
            return builder.RegisterType<TemplateController>();
        }

        protected virtual IRegistrationBuilder<BackupController, Activator, Style> RegisterBackupController(ContainerBuilder builder)
        {
            return builder.RegisterType<BackupController>();
        }
    }
}
