using System;

namespace AstralKeks.Workbench.Core.Management
{
    public class InstallationManager
    {
        private readonly FileSystemManager _fileSystemManager;
        private readonly UserspaceManager _userspaceManager;
        private readonly ConfigurationManager _configurationManager;

        public InstallationManager(FileSystemManager fileSystemManager, UserspaceManager userspaceManager, 
            ConfigurationManager configurationManager)
        {
            _fileSystemManager = fileSystemManager ?? throw new ArgumentNullException(nameof(fileSystemManager));
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
        }

        public void InstallVariables()
        {
            var path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
            var binDirectory = _fileSystemManager.GetBinDirectoryPath();
            if (path.IndexOf(binDirectory, StringComparison.OrdinalIgnoreCase) < 0)
                Environment.SetEnvironmentVariable("Path", $"{path};{binDirectory}", EnvironmentVariableTarget.User);
        }

        public void UninstallVariables()
        {
            var path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
            var binDirectory = _fileSystemManager.GetBinDirectoryPath();
            var binDirectorySemicolon = $"{binDirectory};";
            var index = path.IndexOf(binDirectorySemicolon, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                path = path.Remove(index, binDirectorySemicolon.Length);
                Environment.SetEnvironmentVariable("Path", path, EnvironmentVariableTarget.User);
            }
            index = path.IndexOf(binDirectory, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                path = path.Remove(index, binDirectory.Length);
                Environment.SetEnvironmentVariable("Path", path, EnvironmentVariableTarget.User);
            }
        }

        public void InstallConfiguration()
        {
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            foreach (var configFile in _configurationManager.GetConfigFiles())
                _configurationManager.CreateConfig(userspaceDirectory, configFile);
        }

        public void UninstallConfiguration()
        {
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            foreach (var configFile in _configurationManager.GetConfigFiles())
                _configurationManager.DeleteConfig(userspaceDirectory, configFile);
        }

    }
}
