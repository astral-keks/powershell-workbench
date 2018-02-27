using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using AstralKeks.Workbench.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Get, Noun.WBUserspace)]
    [OutputType(new[] { typeof(Userspace) })]
    public class GetUserspaceCmdlet : WorkbenchCmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty, ArgumentCompleter(typeof(ParameterCompleter))]
        public string UserspaceName { get; set; }

        protected override void ProcessRecord()
        {
            var userspaces = Components.UserspaceRepository.GetUserspaces()
                .Where(u => string.Equals(u.Name, UserspaceName, StringComparison.OrdinalIgnoreCase))
                .ToList();
            WriteObject(userspaces, true);
        }

        [ParameterCompleter(nameof(UserspaceName))]
        public IEnumerable<string> CompleteUserspaceName(string userspaceNamePart)
        {
            return Components.UserspaceRepository.GetUserspaces().Select(u => u.Name);
        }
    }
}
