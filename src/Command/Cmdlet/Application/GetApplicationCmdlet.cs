using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using AstralKeks.Workbench.Models;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Get, Noun.WBApplication)]
    [OutputType(typeof(Application))]
    public class GetApplicationCmdlet : WorkbenchCmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty, ArgumentCompleter(typeof(ParameterCompleter))]
        public string ApplicationName { get; set; }

        protected override void ProcessRecord()
        {
            var applications = Components.ApplicationRepository.GetApplications();
            if (!string.IsNullOrWhiteSpace(ApplicationName))
                applications = applications.Where(a => string.Equals(a.Name, ApplicationName)).ToArray();
            WriteObject(applications, true);
        }

        [ParameterCompleter(nameof(ApplicationName))]
        public IEnumerable<string> CompleteApplicationName(string userspaceNamePart)
        {
            return Components.ApplicationRepository.GetApplications().Select(u => u.Name);
        }
    }
}
