using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using AstralKeks.Workbench.Models;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command.BackingUp
{
    [Cmdlet(VerbsData.Restore, Noun.WBItem)]
    public class RestoreItemCmdlet : WorkbenchCmdlet
    {
        [Parameter(Position = 0)]
        [ArgumentCompleter(typeof(ParameterCompleter))]
        public string BackupName { get; set; }

        [Parameter(Position = 1)]
        [ArgumentCompleter(typeof(ParameterCompleter))]
        public string BackupId { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        protected override void ProcessRecord()
        {
            Components.BackupController.Restore(BackupName, BackupId, ShouldBackup, CanOverwrite);
        }

        private bool ShouldBackup(Backup backup, string backupId)
        {
            return Force || ShouldContinue(
                $"Do you want to make backup before restoration of backup '{backup.Name}' with id '{backupId}'?",
                $"Confirmation required.");
        }

        private bool CanOverwrite(Backup backup, string backupId)
        {
            return Force || ShouldContinue(
                $"Backup '{backup.Name}' with ID '{backupId}' already exists. Do you want to overwrite it?",
                $"Confirmation required.");
        }

        [ParameterCompleter(nameof(BackupName))]
        private IEnumerable<string> CompleteBackupName(string backupNamePart)
        {
            return Components.BackupRepository.GetBackups().Select(b => b.Name);
        }
    }
}
