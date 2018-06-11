using AstralKeks.Workbench.Bootstrappers;
using AutoFixture;

namespace AstralKeks.Workbench.Fixtures
{
    internal class AutoSetupCustomization : ICustomization
    {
        private readonly ComponentContainer _components = new ComponentContainer(new TestContainerBootstrapper());

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(_components.Get<ComponentResolver>());
            fixture.Customize(_components.Get<DataCustomization>());
        }
    }
}
 