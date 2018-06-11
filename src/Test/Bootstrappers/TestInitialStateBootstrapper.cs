using System;
using AstralKeks.Workbench.Context;
using AstralKeks.Workbench.Infrastructure;
using AstralKeks.Workbench.Repositories;
using AstralKeks.Workbench.Utilities;
using Bogus;

namespace AstralKeks.Workbench.Bootstrappers
{
    internal class TestInitialStateBootstrapper : InitialStateBootstrapper
    {
        private readonly FileSystem _fileSystem;
        private readonly SystemVariableMockup _systemVariable;

        public TestInitialStateBootstrapper(FileSystem fileSystem, SystemVariableMockup systemVariable, SessionContext sessionContext, 
            UserspaceRepository userspaceRepository, WorkspaceRepository workspaceRepository) 
            : base(sessionContext, userspaceRepository, workspaceRepository)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _systemVariable = systemVariable ?? throw new ArgumentNullException(nameof(systemVariable));
        }

        public override void Start()
        {
            _systemVariable.WorkspaceDirectory = new Faker().System.PSFilePath(_fileSystem.Separator);
            _systemVariable.HomeUniversal = new Faker().System.PSFilePath(_fileSystem.Separator);

            base.Start();
        }
    }
}
