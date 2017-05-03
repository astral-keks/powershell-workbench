using System;

namespace AstralKeks.Workbench.Core.Management
{
    public class InstallationManager
    {
        private readonly FileSystemManager _fileSystemManager;

        public InstallationManager(FileSystemManager fileSystemManager)
        {
            _fileSystemManager = fileSystemManager ?? throw new ArgumentNullException(nameof(fileSystemManager));
        }

        public string PerformInstallation()
        {
            var path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
            var binDirectory = _fileSystemManager.GetBinDirectoryPath();
            if (!path.Contains(binDirectory))
                Environment.SetEnvironmentVariable("Path", $"{path};{binDirectory}", EnvironmentVariableTarget.User);

            return binDirectory;
        }

    }
}
