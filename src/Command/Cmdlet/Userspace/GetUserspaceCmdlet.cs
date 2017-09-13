using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.Workbench.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Get, Noun.WBUserspace)]
    [OutputType(typeof(Userspace))]
    public class GetUserspaceCmdlet : WorkbenchDynamicCmdlet
    {
        [DynamicParameter(Position = 0)]
        [ValidateNotNullOrEmpty, DynamicCompleter(nameof(CompleteUserspaceName))]
        public string Name => Parameters.GetValue<string>(nameof(Name));

        protected override void ProcessRecord()
        {
            var userspaces = Components.UserspaceRepository.GetUserspaces()
                .Where(u => string.Equals(u.Name, Name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            WriteObject(userspaces, true);
        }

        public IEnumerable<string> CompleteUserspaceName(string userspaceNamePart)
        {
            return Components.UserspaceRepository.GetUserspaces().Select(u => u.Name);
        }
    }
}
