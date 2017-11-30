using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using AstralKeks.Workbench.Models;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command.BackingUp
{
    [Cmdlet(VerbsData.Backup, Noun.WBItem)]
    public class BackupItemCmdlet : WorkbenchCmdlet
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
            Components.BackupController.Backup(BackupName, BackupId, CanOverwrite);
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
