using AstralKeks.Workbench.Fixtures;
using Autofac;
using Bogus;

namespace AstralKeks.Workbench.Bootstrappers
{
    public class TestContainerBootstrapper : ContainerBootstrapper
    {
        protected override void BootstrapContainer(ContainerBuilder builder)
        {
            base.BootstrapContainer(builder);
            BootstrapCustomizations(builder);
        }

        protected override void BootstrapInfrastructure(ContainerBuilder builder)
        {
            builder.RegisterModule<TestInfrastructureBootstrapper>();
        }

        protected override void BootstrapRepositories(ContainerBuilder builder)
        {
            builder.RegisterModule<TestRepositoryBootstrapper>();
        }

        protected override void BootstrapInitialState(ContainerBuilder builder)
        {
            builder.RegisterType<TestInitialStateBootstrapper>().As<IStartable>().SingleInstance();
        }

        private void BootstrapCustomizations(ContainerBuilder builder)
        {
            builder.RegisterType<Faker>().AsSelf();
            builder.RegisterType<ComponentResolver>().AsSelf();
            builder.RegisterType<DataCustomization>().AsSelf();
        }
    }
}
