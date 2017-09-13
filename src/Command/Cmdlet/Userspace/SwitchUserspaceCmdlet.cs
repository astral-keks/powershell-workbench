using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.Workbench.Models;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Switch, Noun.WBUserspace)]
    public class SwitchUserspaceCmdlet : WorkbenchDynamicPSCmdlet
    {
        [DynamicParameter(Position = 0)]
        [ValidateNotNullOrEmpty, DynamicCompleter(nameof(CompleteUserspaceName))]
        public string Name => Parameters.GetValue<string>(nameof(Name));

        [DynamicParameter]
        public SwitchParameter Force => Parameters.GetValue<SwitchParameter>(nameof(Force));

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
                var userspace = Components.UserspaceController.EnterUserspace(Name, () =>
                {
                    Userspace u = null;
                    if (Force || ShouldContinue($"Do you want to create userspace {Name}?", $"Userspace {Name} does not exist."))
                        u = Components.UserspaceRepository.CreateUserspace(Name);
                    return u;
                },
                () => _prevUserspace);
                SessionState.Update(userspace?.Profile);

                var workspace = Components.WorkspaceController.EnterWorkspace(_prevWorkspace);
                SessionState.Update(workspace?.Profile, workspace?.Directory);
            });
        }

        public IEnumerable<string> CompleteUserspaceName(string userspaceNamePart)
        {
            return Components.UserspaceRepository.GetUserspaces().Select(u => u.Name);
        }
    }
}
