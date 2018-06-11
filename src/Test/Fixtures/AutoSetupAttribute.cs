using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Xunit2;

namespace AstralKeks.Workbench.Fixtures
{
    public class AutoSetupAttribute : AutoDataAttribute
    {
        private readonly int _count;

        public AutoSetupAttribute(int count = 5)
            : base(() => new Fixture().Customize(new AutoSetupCustomization()))
        {
            _count = count;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return Enumerable.Repeat(0, _count).SelectMany(z => base.GetData(testMethod));
        }
    }
}
