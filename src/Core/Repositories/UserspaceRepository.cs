using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.Workbench.Repositories
{
    public class UserspaceRepository
    {
        private readonly GlobalContext _globalContext;
        private readonly FileSystem _fileSystem;
        private readonly ProfileRepository _profileRepository;
        private readonly ResourceRepository _resourceRepository;
        
        public UserspaceRepository(GlobalContext globalContext, FileSystem fileSystem, 
            ProfileRepository profileRepository, ResourceRepository resourceRepository)
        {
            _globalContext = globalContext ?? throw new ArgumentNullException(nameof(globalContext));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }
        
        public Userspace CreateUserspace(string userspaceName)
        {
            var userspace = DefineUserspace(userspaceName, null);
            return CreateUserspace(userspace);
        }
        
        public Userspace CreateUserspace(Userspace userspace)
        {
            if (userspace != null)
            {
                _fileSystem.DirectoryCreate(userspace.Directory);

                var userspaceProfilePath = _profileRepository.CurrentUserspace(userspace.Directory);
                _resourceRepository.CreateResource(userspaceProfilePath, Files.UserspacePs1);

                var allUserspacesProfilePath = _profileRepository.AllUserspaces();
                _resourceRepository.CreateResource(allUserspacesProfilePath, Files.AllUserspacesPs1);

                var allWorkspacesProfile = _profileRepository.AllWorkspaces();
                _resourceRepository.CreateResource(allWorkspacesProfile, Files.AllWorkspacesPs1);

                var workspacesProfilePath = _profileRepository.Workspaces(userspace.Directory);
                _resourceRepository.CreateResource(workspacesProfilePath, Files.WorkspacesPs1);


                var configDirectory = PathBuilder.Combine(userspace.Directory, Directories.Config, Directories.Workbench);
                _fileSystem.DirectoryCreate(configDirectory);

                var discoveryConfigPath = PathBuilder.Combine(configDirectory, Files.DiscoveryJson);
                _resourceRepository.CreateResource(discoveryConfigPath, Files.DiscoveryUSJson);

                var shortcutConfigPath = PathBuilder.Combine(configDirectory, Files.ShortcutJson);
                _resourceRepository.CreateResource(shortcutConfigPath, Files.ShortcutUSJson);

                var applicationConfigPath = PathBuilder.Combine(configDirectory, Files.ApplicationJson);
                _resourceRepository.CreateResource(applicationConfigPath, Files.ApplicationUSJson);

                var backupConfigPath = PathBuilder.Combine(configDirectory, Files.BackupJson);
                _resourceRepository.CreateResource(backupConfigPath, Files.BackupUSJson);
            }

            return userspace;
        }

        public IEnumerable<Userspace> GetUserspaces()
        {
            return _fileSystem.GetDirectories(_globalContext.UserspacesDirectory)
                .Select(d => DefineUserspace(null, d))
                .Where(u => _fileSystem.FileExists(u.Marker));
        }

        public Userspace DefineUserspace(string userspaceDirectory)
        {
            return DefineUserspace(null, userspaceDirectory);
        }

        public Userspace DefineUserspace(string userspaceName, string userspaceDirectory)
        {
            Userspace userspace = null;

            if (!string.IsNullOrWhiteSpace(userspaceName) ^ !string.IsNullOrWhiteSpace(userspaceDirectory))
            {
                if (string.IsNullOrWhiteSpace(userspaceName))
                    userspaceName = Path.GetFileNameWithoutExtension(userspaceDirectory);
                if (string.IsNullOrWhiteSpace(userspaceDirectory))
                    userspaceDirectory = PathBuilder.Combine(_globalContext.UserspacesDirectory, userspaceName);
                userspaceDirectory = _fileSystem.GetFullPath(userspaceDirectory);

                var profiles = new[]
                {
                    PathBuilder.Complete(_globalContext.UserspacesDirectory, Files.AllUserspacesPs1),
                    PathBuilder.Complete(userspaceDirectory, Files.UserspacePs1)
                };

                var marker = PathBuilder.Complete(userspaceDirectory, Files.UserspacePs1);

                userspace = new Userspace(userspaceDirectory, marker, profiles);
            }

            return userspace;
        }
    }
}
