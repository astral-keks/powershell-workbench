﻿using AstralKeks.Workbench.Models;
using System.Management.Automation;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.New, Noun.WBWorkspace)]
    [OutputType(typeof(Workspace))]
    public class NewWorkspaceCmdlet : WorkbenchCmdlet
    {
        [Parameter(Position = 0)]
        public string Directory { get; }

        protected override void ProcessRecord()
        {
            var workspace = Components.WorkspaceRepository.CreateWorkspace(Directory);
            WriteObject(workspace);
        }
    }
}
