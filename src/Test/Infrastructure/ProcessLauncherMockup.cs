using System.Collections.Generic;
using System.Diagnostics;

namespace AstralKeks.Workbench.Infrastructure
{
    public class ProcessLauncherMockup : ProcessLauncher
    {
        public List<ProcessStartInfo> Processes { get; } = new List<ProcessStartInfo>();
        public List<ProcessStartInfo> Completed { get; } = new List<ProcessStartInfo>();

        public override void Launch(ProcessStartInfo processInfo, bool waitForExit)
        {
            Processes.Add(processInfo);
            if (waitForExit)
                Completed.Add(processInfo);
        }
    }
}
