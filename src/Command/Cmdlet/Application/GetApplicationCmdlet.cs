using AstralKeks.PowerShell.Common.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Get, Noun.WBApplication)]
    public class GetApplicationCmdlet : WorkbenchDynamicCmdlet
    {
        [DynamicParameter(Position = 0)]
        [ValidateNotNullOrEmpty, DynamicCompleter(nameof(CompleteApplicationName))]
        public string Name => Parameters.GetValue<string>(nameof(Name));

        protected override void ProcessRecord()
        {
            var applications = Components.ApplicationRepository.GetApplications();
            if (!string.IsNullOrWhiteSpace(Name))
                applications = applications.Where(a => string.Equals(a.Name, Name)).ToArray();
            WriteObject(applications, true);
        }

        public IEnumerable<string> CompleteApplicationName(string userspaceNamePart)
        {
            return Components.ApplicationRepository.GetApplications().Select(u => u.Name);
        }
    }
}
