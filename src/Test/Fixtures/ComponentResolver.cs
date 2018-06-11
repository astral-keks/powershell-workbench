using Autofac;
using AutoFixture.Kernel;
using System;
using System.Collections;

namespace AstralKeks.Workbench.Fixtures
{
    internal class ComponentResolver : ISpecimenBuilder
    {
        private readonly IComponentContext _context;

        public ComponentResolver(IComponentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public object Create(object request, ISpecimenContext context)
        {
            object result = new NoSpecimen();
            if (request is Type)
            {
                var type = request as Type;
                if (!type.IsAssignableTo<IEnumerable>() && _context.IsRegistered(type))
                    result = _context.ResolveOptional(type) ?? result;
            }
            return result;
        }
    }
}
