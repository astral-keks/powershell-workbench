using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.FileSystem;
using System;

namespace AstralKeks.Workbench.Core.Management
{
    public class InstallationManager
    {
        private readonly UserspaceManager _userspaceManager;
        private readonly ConfigurationManager _configurationManager;

        public InstallationManager(UserspaceManager userspaceManager, ConfigurationManager configurationManager)
        {
            _userspaceManager = userspaceManager ?? throw new ArgumentNullException(nameof(userspaceManager));
            _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
        }

        public void InstallVariables()
        {
            var path = SystemVariable.Path;
            var binDirectory = FsPath.BinDirectory();

            if (path.IndexOf(binDirectory, StringComparison.OrdinalIgnoreCase) < 0)
                SystemVariable.Path = $"{path};{binDirectory}";
        }

        public void UninstallVariables()
        {
            var path = SystemVariable.Path;
            var binDirectory = FsPath.BinDirectory();
            var binDirectorySemicolon = $"{binDirectory};";
            var index = path.IndexOf(binDirectorySemicolon, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                path = path.Remove(index, binDirectorySemicolon.Length);
                SystemVariable.Path = path;
            }
            index = path.IndexOf(binDirectory, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                path = path.Remove(index, binDirectory.Length);
                SystemVariable.Path = path;
            }
        }

        public void InstallConfiguration()
        {
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            foreach (var configFile in _configurationManager.GetConfigFileNames())
                _configurationManager.CreateConfig(userspaceDirectory, configFile);
        }

        public void UninstallConfiguration()
        {
            var userspaceDirectory = _userspaceManager.GetUserspaceDirectory();
            foreach (var configFile in _configurationManager.GetConfigFileNames())
                _configurationManager.DeleteConfig(userspaceDirectory, configFile);
        }
    }
}
