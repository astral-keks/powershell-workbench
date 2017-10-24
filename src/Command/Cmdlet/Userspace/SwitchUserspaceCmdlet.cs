using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using AstralKeks.Workbench.Models;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Switch, Noun.WBUserspace)]
    public class SwitchUserspaceCmdlet : WorkbenchPSCmdlet
    {
        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty, ArgumentCompleter(typeof(ParameterCompleter))]
        public string UserspaceName { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        private Userspace _prevUserspace;
        private Workspace _prevWorkspace;

        protected override void BeginProcessing()
        {
            SessionState.Restore(() =>
            {
                _prevWorkspace = Components.WorkspaceController.ExitWorkspace();
                _prevUserspace = Components.UserspaceController.ExitUserspace();
            });
        }

        protected override void EndProcessing()
        {
            SessionState.Update(() =>
            {
                var userspace = Components.UserspaceController.EnterUserspace(UserspaceName, () =>
                {
                    Userspace u = null;
                    if (Force || ShouldContinue($"Do you want to create userspace {UserspaceName}?", $"Userspace {UserspaceName} does not exist."))
                        u = Components.UserspaceRepository.CreateUserspace(UserspaceName);
                    return u;
                },
                () => _prevUserspace);
                SessionState.Update(userspace?.Profile);

                var workspace = Components.WorkspaceController.EnterWorkspace(_prevWorkspace);
                SessionState.Update(workspace?.Profile, workspace?.Directory);
            });
        }

        [ParameterCompleter(nameof(UserspaceName))]
        public IEnumerable<string> CompleteUserspaceName(string userspaceNamePart)
        {
            return Components.UserspaceRepository.GetUserspaces().Select(u => u.Name);
        }
    }
}
