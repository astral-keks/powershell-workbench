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
            RegisterGlobalContext(builder).As<GlobalContext>().SingleInstance();
            RegisterSessionContext(builder).As<SessionContext>().SingleInstance();
            RegisterTemplateProcessor(builder).As<TemplateProcessor>().SingleInstance();
            RegisterResourceRepository(builder).As<ResourceRepository>().SingleInstance();
        }

        protected virtual IRegistrationBuilder<GlobalContext, Activator, Style> RegisterGlobalContext(ContainerBuilder builder)
        {
            return builder.RegisterType<GlobalContext>();
        }

        protected virtual IRegistrationBuilder<SessionContext, Activator, Style> RegisterSessionContext(ContainerBuilder builder)
        {
            return builder.RegisterType<SessionContext>();
        }

        protected virtual IRegistrationBuilder<TemplateProcessor, Activator, Style> RegisterTemplateProcessor(ContainerBuilder builder)
        {
            return builder.RegisterType<TemplateProcessor>();
        }

        protected virtual IRegistrationBuilder<ResourceRepository, Activator, Style> RegisterResourceRepository(ContainerBuilder builder)
        {
            return builder.RegisterType<ResourceRepository>();
        }
    }
}
