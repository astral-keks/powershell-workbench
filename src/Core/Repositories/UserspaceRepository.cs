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
        private readonly UserspaceContext _context;
        private readonly FileSystem _fileSystem;
        private readonly ResourceRepository _resourceRepository;
        
        public UserspaceRepository(UserspaceContext context, FileSystem fileSystem, ResourceRepository resourceRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }
        
        public Userspace CreateUserspace(string userspaceName)
        {
            var userspace = GetUserspace(userspaceName, null);
            return CreateUserspace(userspace);
        }
        
        public Userspace CreateUserspace(Userspace userspace)
        {
            if (userspace != null)
            {
                var configDirectory = PathBuilder.Combine(userspace.Directory, Directories.Config, Directories.Workbench);

                _fileSystem.DirectoryCreate(userspace.Directory);
                _fileSystem.DirectoryCreate(configDirectory);

                _resourceRepository.CreateResource(userspace.Profile, Files.UserspacePs1);

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
            return _fileSystem.GetDirectories(_context.UserspaceRootDirectory)
                .Select(d => GetUserspace(null, d))
                .Where(u => _fileSystem.FileExists(u.Profile));
        }

        private Userspace GetUserspace(string userspaceName = null, string userspaceDirectory = null)
        {
            Userspace userspace = null;

            if (!string.IsNullOrWhiteSpace(userspaceName) ^ !string.IsNullOrWhiteSpace(userspaceDirectory))
            {
                if (string.IsNullOrWhiteSpace(userspaceName))
                    userspaceName = Path.GetFileNameWithoutExtension(userspaceDirectory);
                if (string.IsNullOrWhiteSpace(userspaceDirectory))
                    userspaceDirectory = PathBuilder.Combine(_context.UserspaceRootDirectory, userspaceName);
                userspaceDirectory = _fileSystem.GetFullPath(userspaceDirectory);

                userspace = new Userspace(userspaceDirectory);
            }

            return userspace;
        }
    }
}
