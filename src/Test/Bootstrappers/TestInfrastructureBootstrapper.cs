using AstralKeks.Workbench.Infrastructure;
using Autofac;
using System.IO.Abstractions.TestingHelpers;

namespace AstralKeks.Workbench.Bootstrappers
{
    internal class TestInfrastructureBootstrapper : InfrastructureBootstrapper
    {
        protected override void RegisterFileSystem(ContainerBuilder builder)
        {
            builder.RegisterType<MockFileSystem>().As<IMockFileDataAccessor>();
            builder.RegisterType<FileSystemMockup>().As<FileSystem>().SingleInstance();
        }

        protected override void RegisterProcessLauncher(ContainerBuilder builder)
        {
            builder.RegisterType<ProcessLauncherMockup>().AsSelf().As<ProcessLauncher>().SingleInstance();
        }

        protected override void RegisterSystemVariable(ContainerBuilder builder)
        {
            builder.RegisterType<SystemVariableMockup>().AsSelf().As<SystemVariable>().SingleInstance();
        }
    }
}
