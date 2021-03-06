using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsOther.Use, Noun.WBUserspace)]
    public class UseUserspaceCmdlet : WorkbenchPSCmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty, ArgumentCompleter(typeof(ParameterCompleter))]
        public string UserspaceName { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }
        
        protected override void ProcessRecord()
        {
            if (!Components.UserspaceController.CheckUserspace(UserspaceName, ShouldCreateUserspace))
                throw new DirectoryNotFoundException($"Userspace {UserspaceName} was not found");

            SessionState.Update(() => Components.UserspaceController.UseUserspace(UserspaceName));
        }

        private bool ShouldCreateUserspace()
        {
            return Force || ShouldContinue(
                $"Do you want to create userspace {UserspaceName}?",
                $"Userspace {UserspaceName} does not exist in user folder.");
        }

        [ParameterCompleter(nameof(UserspaceName))]
        private IEnumerable<string> CompleteUserspaceName(string userspaceNamePart)
        {
            return Components.UserspaceRepository.GetUserspaces().Select(u => u.Name);
        }
    }
}
