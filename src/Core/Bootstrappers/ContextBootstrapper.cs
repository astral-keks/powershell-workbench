using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using Autofac;
using Autofac.Builder;
using Style = Autofac.Builder.SingleRegistrationStyle;
using Activator = Autofac.Builder.ConcreteReflectionActivatorData;
using AstralKeks.Workbench.Common.Template;

namespace AstralKeks.Workbench.Bootstrappers
{
    public class ContextBootstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterUserspaceContext(builder).As<UserspaceContext>().SingleInstance();
            RegisterWorkspaceContext(builder).As<WorkspaceContext>().SingleInstance();
            RegisterTemplateProcessor(builder).As<TemplateProcessor>().SingleInstance();
            RegisterResourceRepository(builder).As<ResourceRepository>().SingleInstance();
            //RegisterResourceBootstrapper(builder).As<IStartable>().SingleInstance();
        }

        protected virtual IRegistrationBuilder<UserspaceContext, Activator, Style> RegisterUserspaceContext(ContainerBuilder builder)
        {
            return builder.RegisterType<UserspaceContext>();
        }

        protected virtual IRegistrationBuilder<WorkspaceContext, Activator, Style> RegisterWorkspaceContext(ContainerBuilder builder)
        {
            return builder.RegisterType<WorkspaceContext>();
        }

        protected virtual IRegistrationBuilder<TemplateProcessor, Activator, Style> RegisterTemplateProcessor(ContainerBuilder builder)
        {
            return builder.RegisterType<TemplateProcessor>();
        }

        protected virtual IRegistrationBuilder<ResourceRepository, Activator, Style> RegisterResourceRepository(ContainerBuilder builder)
        {
            return builder.RegisterType<ResourceRepository>();
        }

        protected virtual IRegistrationBuilder<ResourceBootstrapper, Activator, Style> RegisterResourceBootstrapper(ContainerBuilder builder)
        {
            return builder.RegisterType<ResourceBootstrapper>();
        }
    }
}
