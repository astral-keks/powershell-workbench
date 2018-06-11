using AstralKeks.Workbench.Repositories;
using Autofac;
using Autofac.Builder;

namespace AstralKeks.Workbench.Bootstrappers
{
    internal class TestRepositoryBootstrapper : RepositoryBootstrapper
    {
        protected override IRegistrationBuilder<ApplicationRepository, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterApplicationRepository(
            ContainerBuilder builder)
        {
            return builder.RegisterType<TestApplicationRepository>();
        }
    }
}
