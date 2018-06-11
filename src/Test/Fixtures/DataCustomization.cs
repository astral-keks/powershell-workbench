using AstralKeks.Workbench.Infrastructure;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Utilities;
using AutoFixture;
using Bogus;
using System;
using System.Linq;


namespace AstralKeks.Workbench.Fixtures
{
    internal class DataCustomization : ICustomization
    {
        private readonly Faker _faker;
        private readonly FileSystem _fileSystem;
        
        public DataCustomization(Faker faker, FileSystem fileSystem)
        {
            _faker = faker ?? throw new ArgumentNullException(nameof(faker));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customize<Application>(c => c
                .Without(app => app.Executable)
                .Do(app => app.Executable = _faker.System.PSFilePath(_fileSystem.Separator))
            );
        }
    }
}
