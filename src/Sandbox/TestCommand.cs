using System;
using System.Management.Automation;

namespace Test
{
    [Cmdlet(VerbsDiagnostic.Test, "Command")]
    [OutputType(typeof(string))]
    public class TestCommand : Cmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("asdasdasd");
        }
    }
}
