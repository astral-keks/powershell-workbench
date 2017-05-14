﻿using AstralKeks.Workbench.PowerShell.Attributes;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.Get, Noun.WBWorkspace)]
    public class GetWorkspaceCmdlet : WorkbenchDynamicCmdlet
    {
        [DynamicParameter(Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Directory => Parameters.GetValue<string>(nameof(Directory));

        protected override void ProcessRecord()
        {
            if (!string.IsNullOrWhiteSpace(Directory))
                WriteObject(Env.WorkspaceManager.GetWorkspaceDirectory(Directory));
            else
                WriteObject(Env.WorkspaceManager.GetWorkspaceDirectory());
        }
    }
}