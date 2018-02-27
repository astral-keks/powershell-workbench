using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Template;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Configuration;
using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AstralKeks.Workbench.Repositories
{
    public class BackupRepository
    {
        private readonly SessionContext _sessionContext;
        private readonly TemplateProcessor _templateProcessor;
        private readonly ResourceRepository _resourceRepository;
        private readonly FileSystem _fileSystem;

        public BackupRepository(SessionContext sessionContext, TemplateProcessor templateProcessor, 
            ResourceRepository resourceRepository, FileSystem fileSystem)
        {
            _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }
        
        public IEnumerable<Backup> GetBackups()
        {
            var workspaceResourcePath = PathBuilder.Complete(
                _sessionContext.CurrentWorkspaceDirectory, Directories.Config, Directories.Workbench, Files.BackupJson);
            var userspaceResourcePath = PathBuilder.Combine(
                _sessionContext.CurrentUserspaceDirectory, Directories.Config, Directories.Workbench, Files.BackupJson);
            var workspaceResource = _templateProcessor.Transform(_resourceRepository.GetResource(workspaceResourcePath));
            var userspaceResource = _templateProcessor.Transform(_resourceRepository.GetResource(userspaceResourcePath));

            var backupConfiguration = userspaceResource?.Read<BackupConfig>(workspaceResource);
            backupConfiguration = backupConfiguration ?? new BackupConfig();
            return backupConfiguration;
        }

        public string GetBackupId(Backup backup = null, string backupId = null)
        {
            if (backupId == null)
                backupId = backup?.Id;
            if (backupId == null)
                backupId = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-ffff");
            return backupId;
        }

        public bool ExistsBackup(Backup backup, string backupId)
        {
            if (backup == null)
                throw new ArgumentNullException(nameof(backup));

            var destinationDirectory = GetBackupDestination(backup, backupId);
            return _fileSystem.DirectoryExists(destinationDirectory);
        }

        public void SaveBackup(Backup backup, string backupId)
        {
            if (backup == null)
                throw new ArgumentNullException(nameof(backup));
            
            ClearBackup(backup, backupId);
            foreach (var source in backup.Sources)
            {
                var destination = GetBackupDestination(backup, backupId, source);
                CopyBackupItems(source, destination);
            }
        }

        public void LoadBackup(Backup backup, string backupId)
        {
            if (backup == null)
                throw new ArgumentNullException(nameof(backup));

            foreach (var source in backup.Sources)
            {
                var destination = GetBackupDestination(backup, backupId, source);
                CopyBackupItems(destination, source);
            }
        }

        private void ClearBackup(Backup backup, string backupId)
        {
            var destination = GetBackupDestination(backup, backupId);
            _fileSystem.DirectoryDelete(destination, true);
        }

        private string GetBackupDestination(Backup backup, string backupId, string source = null)
        {
            backupId = GetBackupId(backup, backupId);
            
            var destinationPayload = GetBackupId(backup, backupId);
            var destinationDirectory = PathBuilder.Complete(backup.Destination, destinationPayload);

            string destinationFileName = null;
            if (!string.IsNullOrWhiteSpace(source))
            {
                source = _fileSystem.GetFullPath(source);
                var directory = Path.GetDirectoryName(source);
                var fileName = Path.GetFileName(source);
                destinationFileName = PathBuilder.Combine(GetMD5Hash(directory), fileName);
            }

            return PathBuilder.Complete(destinationDirectory, destinationFileName);
        }

        private void CopyBackupItems(string source, string destination)
        {
            if (!string.IsNullOrWhiteSpace(source) && !string.IsNullOrWhiteSpace(destination))
            {
                if (_fileSystem.DirectoryExists(source))
                {
                    var entries = _fileSystem.DirectoryList(source);
                    foreach (var sourceEntry in entries)
                    {
                        var sourceEntryFileName = Path.GetFileName(sourceEntry);
                        var destinationEntry = PathBuilder.Complete(destination, sourceEntryFileName);
                        CopyBackupItems(sourceEntry, destinationEntry);
                    }
                }
                else if (_fileSystem.FileExists(source))
                {
                    _fileSystem.FileClone(source, destination, true);
                }
            }
        }
        
        private string GetMD5Hash(string source)
        {
            using (var md5 = MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(source);
                var hashBytes = md5.ComputeHash(sourceBytes);
                return string.Concat(hashBytes.Select(b => b.ToString("x2")));
            }
        }
    }
}
