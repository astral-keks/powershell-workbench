using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using AstralKeks.Workbench.Models;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.New, Noun.WBUserspace)]
    [OutputType(typeof(Userspace))]
    public class NewUserspaceCmdlet : WorkbenchCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty, ArgumentCompleter(typeof(ParameterCompleter))]
        public string UserspaceName { get; set; }

        protected override void ProcessRecord()
        {
            var userspace = Components.UserspaceRepository.CreateUserspace(UserspaceName);
            WriteObject(userspace);
        }

        [ParameterCompleter(nameof(UserspaceName))]
        public IEnumerable<string> CompleteUserspaceName(string userspaceNamePart)
        {
            return Components.UserspaceRepository.GetUserspaces().Select(u => u.Name);
        }
    }
}
