using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.Workbench.Models;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.New, Noun.WBUserspace)]
    [OutputType(typeof(Userspace))]
    public class NewUserspaceCmdlet : WorkbenchDynamicCmdlet
    {
        [DynamicParameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty, DynamicCompleter(nameof(CompleteUserspaceName))]
        public string Name => Parameters.GetValue<string>(nameof(Name));

        protected override void ProcessRecord()
        {
            var userspace = Components.UserspaceRepository.CreateUserspace(Name);
            WriteObject(userspace);
        }

        public IEnumerable<string> CompleteUserspaceName(string userspaceNamePart)
        {
            return Components.UserspaceRepository.GetUserspaces().Select(u => u.Name);
        }
    }
}
