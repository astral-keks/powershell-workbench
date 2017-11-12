using AstralKeks.Workbench.Models;
using AstralKeks.Workbench.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace AstralKeks.Workbench.Controllers
{
    public class BackupController
    {
        public delegate bool ConfirmCallback(Backup backup, string backupId);


        private readonly BackupRepository _backupRepository;

        public BackupController(BackupRepository backupRepository)
        {
            _backupRepository = backupRepository ?? throw new System.ArgumentNullException(nameof(backupRepository));
        }

        public void Backup(string backupName, string backupId, ConfirmCallback canOverwrite)
        {
            var backups = GetBackups(backupName);
            foreach (var backup in backups)
            {
                Backup(backup, backupId, canOverwrite);
            }
        }

        public void Restore(string backupName, string backupId, ConfirmCallback shouldBackup, ConfirmCallback canOverwrite)
        {
            var backups = GetBackups(backupName);
            foreach (var backup in backups)
            {
                Restore(backup, backupId, shouldBackup, canOverwrite);
            }
        }

        private void Backup(Backup backup, string backupId, ConfirmCallback canOverwrite)
        {
            backupId = _backupRepository.GetBackupId(backup, backupId);

            var backupExists = _backupRepository.ExistsBackup(backup, backupId);
            if (!backupExists || canOverwrite(backup, backupId))
                _backupRepository.SaveBackup(backup, backupId);
        }

        private void Restore(Backup backup, string backupId, ConfirmCallback shouldBackup, ConfirmCallback canOverwrite)
        {
            backupId = _backupRepository.GetBackupId(backup, backupId);

            if (shouldBackup(backup, backupId))
            {
                var newBackupId = _backupRepository.GetBackupId();
                Backup(backup, newBackupId, canOverwrite);
            }

            _backupRepository.LoadBackup(backup, backupId);
        }

        private IEnumerable<Backup> GetBackups(string backupName)
        {
            var backups = _backupRepository.GetBackups();
            if (!string.IsNullOrWhiteSpace(backupName))
                backups = backups.Where(b => b.Name == backupName);

            return backups;
        }
    }
}
