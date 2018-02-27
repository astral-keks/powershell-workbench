using System;
using System.Management.Automation;

namespace AstralKeks.Workbench.Sandbox
{
    [Cmdlet(VerbsDiagnostic.Test, "Command")]
    [OutputType(new[] { typeof(string) })]
    public class TestCommand : Cmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("asdasdasdss");
        }
    }
}
